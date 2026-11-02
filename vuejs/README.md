# PacientIK Vue

Vue 3 + TypeScript client for the PacientIK API.

## Run locally

1. Start the API with its HTTP launch profile (`http://localhost:5299`).
2. In this directory, install dependencies and start Vite:

   ```sh
   npm install
   npm run dev
   ```

3. Open the Vite URL shown in the terminal and sign in with an API account.

Vite proxies `/api` requests to `http://localhost:5299`. Set `VITE_API_PROXY_TARGET` in a local `.env` file to use another API address. For a deployed API, set `VITE_API_URL` to its `/api` base URL; the API must allow the frontend origin and credentials.

The client uses the API's HttpOnly `token` cookie. It does not read or store the JWT in browser storage.
