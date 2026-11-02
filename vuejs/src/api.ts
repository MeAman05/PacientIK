export interface Session {
  token: string
  userId: string
  spec: string
  role: string
}

export interface CurrentUser {
  uid: string
  specId: string
  role: string
}

export interface User {
  userid: string
  name: string
  surname: string
  lastname: string
  age: number
  photourl: string
  spec: string
  role: string
}

export interface Report {
  id: number
  pacientname: string
  sender: string
  price: number
  leches: string[]
  lechids: number[]
  period: number
}

export interface Speciality {
  id: number
  name: string
}

export interface Service {
  id: number
  spec: string
  name: string
}

const apiBase = import.meta.env.VITE_API_URL || '/api'

async function request<T>(path: string, init: RequestInit = {}): Promise<T> {
  const headers = new Headers(init.headers)
  if (init.body && !(init.body instanceof FormData) && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json')
  }

  const response = await fetch(`${apiBase}${path}`, {
    ...init,
    headers,
    credentials: 'include',
  })

  if (!response.ok) {
    const body = await response.text()
    let message = body || `Ошибка запроса (${response.status})`
    try {
      const problem = JSON.parse(body) as { title?: string; detail?: string }
      message = problem.detail || problem.title || message
    } catch {
      // The API may return a plain-text error response.
    }
    throw new Error(message)
  }

  if (response.status === 204) return undefined as T
  const body = await response.text()
  if (!body) return undefined as T
  if (response.headers.get('Content-Type')?.includes('application/json')) {
    return JSON.parse(body) as T
  }
  return body as T
}

export const api = {
  login: (code: string, password: string) =>
    request<Session>('/login/login', {
      method: 'POST',
      body: JSON.stringify({ code, password }),
    }),
  logout: () => request<void>('/login/logout', { method: 'POST' }),
  me: () => request<CurrentUser>('/login/logme'),
  reports: (role: string, name = '') => {
    const query = new URLSearchParams({ name, lechid: '0' })
    if (role === 'Admin' || role === 'Checker') {
      query.set('snder', '')
      return request<Report[]>(`/doc/getallreports?${query}`)
    }
    return request<Report[]>(`/doc/getownreports?${query}`)
  },
  reportById: (id: number) => request<Report>(`/doc/getreportbyid/${id}`),
  addReport: (report: {
    pacientname: string
    price: number
    lechids: number[]
    period: number
  }) =>
    request<Report>('/doc/addreport', {
      method: 'POST',
      body: JSON.stringify(report),
    }),
  updateReport: (
    id: number,
    report: { pacientname: string; price: number; lechids: number[]; period: number },
  ) =>
    request<Report>(`/doc/update/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(report),
    }),
  deleteReport: (id: number) =>
    request<string>(`/doc/delete/${id}`, { method: 'DELETE' }),
  users: (text: string) =>
    request<User[]>(`/user/getusers?${new URLSearchParams({ text })}`),
  userById: (id: string) => request<User>(`/user/getuserbyid/${encodeURIComponent(id)}`),
  addUser: (values: FormData) =>
    request<User>('/user/create', { method: 'POST', body: values }),
  updateUser: (id: string, values: FormData) =>
    request<unknown>(`/user/update/${encodeURIComponent(id)}`, { method: 'PATCH', body: values }),
  deleteUser: (id: string) =>
    request<string>(`/user/delete/${id}`, { method: 'DELETE' }),
  specialities: () => request<Speciality[]>('/spec/getallspeces'),
  addSpeciality: (name: string) =>
    request<Speciality>('/spec/addspec', {
      method: 'POST',
      body: JSON.stringify({ name }),
    }),
  deleteSpeciality: (id: number) =>
    request<string>(`/spec/deletespec/${id}`, { method: 'DELETE' }),
  services: () => request<Service[]>('/lech/getallleches'),
  addService: (name: string, spec: number) =>
    request<Service>('/lech/addlech', {
      method: 'POST',
      body: JSON.stringify({ name, spec }),
    }),
  deleteService: (id: number) =>
    request<string>(`/lech/deletelech/${id}`, { method: 'DELETE' }),
}
