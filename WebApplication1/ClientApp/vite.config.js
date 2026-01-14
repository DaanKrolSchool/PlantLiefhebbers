import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            '/inlog': 'https://plantliefhebbers-c6esfzdnfaf2cdat.swedencentral-01.azurewebsites.net',
            '/klantregister': 'https://plantliefhebbers-c6esfzdnfaf2cdat.swedencentral-01.azurewebsites.net',
            '/Product': 'https://plantliefhebbers-c6esfzdnfaf2cdat.swedencentral-01.azurewebsites.net',
            '/api': 'https://plantliefhebbers-c6esfzdnfaf2cdat.swedencentral-01.azurewebsites.net',
            '/images': 'https://plantliefhebbers-c6esfzdnfaf2cdat.swedencentral-01.azurewebsites.net'
        }
    }
})
    