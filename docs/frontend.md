## Frontend HTTP Client Libraries for Next.js

### Recommended Libraries

1. **Axios** - Popular HTTP client with interceptors support
```bash
npm install axios
```

2. **TanStack Query (React Query)** - Data fetching with caching, mutations, and state management
```bash
npm install @tanstack/react-query
```

### Setup

```typescript
// lib/api-client.ts
import axios from 'axios';

export const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('access_token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      // Handle token refresh or redirect to login
    }
    return Promise.reject(error);
  }
);
```

```typescript
// providers/query-provider.tsx
'use client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useState } from 'react';

export function QueryProvider({ children }: { children: React.ReactNode }) {
  const [queryClient] = useState(() => new QueryClient());
  return (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
}
```

### TanStack Query vs SWR Comparison

| Feature | TanStack Query | SWR |
|---------|---------------|-----|
| **Bundle Size** | ~13kb | ~4kb |
| **Mutations** | Built-in `useMutation` hook | Manual implementation |
| **Devtools** | Official React Query Devtools | Community devtools |
| **Infinite Queries** | `useInfiniteQuery` | `useSWRInfinite` |
| **Prefetching** | `queryClient.prefetchQuery` | `preload` / `mutate` |
| **Optimistic Updates** | First-class support | Manual with `mutate` |
| **Garbage Collection** | Automatic with configurable time | Manual |
| **Parallel Queries** | `useQueries` | Multiple `useSWR` calls |
| **Dependent Queries** | `enabled` option | Conditional fetching |
| **Cache Persistence** | Plugin support | Plugin support |

**Choose TanStack Query if:**
- Complex mutations with rollback
- Need built-in devtools
- Require fine-grained cache control

**Choose SWR if:**
- Bundle size is critical
- Simpler data fetching needs
- Prefer Vercel ecosystem

```typescript
// SWR Example
import useSWR from 'swr';

const fetcher = (url: string) => apiClient.get(url).then(res => res.data);

function useUser(id: string) {
  return useSWR(`/users/${id}`, fetcher);
}

// TanStack Query Example
import { useQuery } from '@tanstack/react-query';

function useUser(id: string) {
  return useQuery({
    queryKey: ['user', id],
    queryFn: () => apiClient.get(`/users/${id}`).then(res => res.data),
  });
}
```

## Production-Ready TanStack Query Implementation

### 1. Query Client Configuration

```typescript
// lib/query-client.ts
import { QueryClient } from '@tanstack/react-query';

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000, // 5 minutes
      gcTime: 10 * 60 * 1000, // 10 minutes (formerly cacheTime)
      retry: 3,
      retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 30000),
      refetchOnWindowFocus: false,
      refetchOnReconnect: true,
    },
    mutations: {
      retry: 1,
    },
  },
});
```

### 2. Provider Setup

```typescript
// providers/query-provider.tsx
'use client';

import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { queryClient } from '@/lib/query-client';

export function QueryProvider({ children }: { children: React.ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {process.env.NODE_ENV === 'development' && <ReactQueryDevtools initialIsOpen={false} />}
    </QueryClientProvider>
  );
}
```

### 3. Custom Hooks with Error Handling

```typescript
// hooks/use-users.ts
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';
import { UserDto } from '@/types';

const userKeys = {
  all: ['users'] as const,
  lists: () => [...userKeys.all, 'list'] as const,
  list: (filters: Record<string, unknown>) => [...userKeys.lists(), filters] as const,
  details: () => [...userKeys.all, 'detail'] as const,
  detail: (id: string) => [...userKeys.details(), id] as const,
};

export function useUsers(filters?: { page?: number; limit?: number }) {
  return useQuery({
    queryKey: userKeys.list(filters ?? {}),
    queryFn: async () => {
      const { data } = await apiClient.get<{ users: UserDto[]; total: number }>('/users', { params: filters });
      return data;
    },
    placeholderData: (previousData) => previousData,
  });
}

export function useUser(id: string) {
  return useQuery({
    queryKey: userKeys.detail(id),
    queryFn: async () => {
      const { data } = await apiClient.get<UserDto>(`/users/${id}`);
      return data;
    },
    enabled: !!id,
  });
}

export function useUpdateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ id, ...updates }: Partial<UserDto> & { id: string }) => {
      const { data } = await apiClient.patch<UserDto>(`/users/${id}`, updates);
      return data;
    },
    onMutate: async ({ id, ...updates }) => {
      await queryClient.cancelQueries({ queryKey: userKeys.detail(id) });
      const previousUser = queryClient.getQueryData<UserDto>(userKeys.detail(id));
      
      queryClient.setQueryData<UserDto>(userKeys.detail(id), (old) => 
        old ? { ...old, ...updates } : old
      );
      
      return { previousUser };
    },
    onError: (err, { id }, context) => {
      if (context?.previousUser) {
        queryClient.setQueryData(userKeys.detail(id), context.previousUser);
      }
    },
    onSettled: (data, error, { id }) => {
      queryClient.invalidateQueries({ queryKey: userKeys.detail(id) });
      queryClient.invalidateQueries({ queryKey: userKeys.lists() });
    },
  });
}

export function useDeleteUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/users/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: userKeys.lists() });
    },
  });
}
```

### 4. Infinite Query for Pagination

```typescript
// hooks/use-infinite-users.ts
import { useInfiniteQuery } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

export function useInfiniteUsers(limit = 20) {
  return useInfiniteQuery({
    queryKey: ['users', 'infinite', limit],
    queryFn: async ({ pageParam = 0 }) => {
      const { data } = await apiClient.get('/users', {
        params: { offset: pageParam, limit },
      });
      return data;
    },
    initialPageParam: 0,
    getNextPageParam: (lastPage, allPages) => {
      return lastPage.users.length === limit ? allPages.length * limit : undefined;
    },
  });
}
```

### 5. Usage in Components

```typescript
// components/user-list.tsx
'use client';

import { useUsers, useDeleteUser } from '@/hooks/use-users';

export function UserList() {
  const { data, isLoading, isError, error } = useUsers({ page: 1, limit: 10 });
  const deleteUser = useDeleteUser();

  if (isLoading) return <div>Loading...</div>;
  if (isError) return <div>Error: {error.message}</div>;

  return (
    <ul>
      {data?.users.map((user) => (
        <li key={user.id}>
          {user.firstName} {user.lastName}
          <button
            onClick={() => deleteUser.mutate(user.id)}
            disabled={deleteUser.isPending}
          >
            {deleteUser.isPending ? 'Deleting...' : 'Delete'}
          </button>
        </li>
      ))}
    </ul>
  );
}
```
