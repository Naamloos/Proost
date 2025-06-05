/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        'beer-amber': '#FFBF00',
        'beer-gold': '#FFD700',
        'beer-foam': '#F7F2E0',
        'app-purple': '#8A2BE2',
        'app-pink': '#FF1493',
        'app-dark': '#121212',
      },
      animation: {
        'float-slow': 'float 15s ease-in-out infinite',
        'float-medium': 'float 12s ease-in-out infinite',
        'float-fast': 'float 9s ease-in-out infinite',
      },
      keyframes: {
        float: {
          '0%, 100%': { transform: 'translateY(0) rotate(0deg)' },
          '25%': { transform: 'translateY(-10px) rotate(5deg)' },
          '50%': { transform: 'translateY(5px) rotate(-5deg)' },
          '75%': { transform: 'translateY(-5px) rotate(2deg)' },
        },
      },
    },
  },
  plugins: [],
};