export const environment = {
    production: false,
    apiUrl: 'https://localhost:5001/api/',
    apiTimeout: 30000, // 30 seconds
    enableDebug: true,
    logging:{
        level: 'debug', // Options: 'debug', 'info', 'warn', 'error'
        enableConsoleLogging : true // Enable or disable console logging
    }
}