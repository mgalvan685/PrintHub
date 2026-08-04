module.exports = {
  content: ["./index.html", "./src/**/*.{ts,tsx}"],
  theme: {
    extend: {},
  },
  plugins: [],
}

export default {
  content: ["./index.html", "./src/**/*.{ts,tsx}"],
  theme: {
    extend: {
      colors: {
        tactical: {
          bg: "#0f1115",
          bg2: "#1a1d23",
          border: "#2a2e36",
          text: "#ffffff",
          text2: "#cbd5e1",
          textMuted: "#94a3b8",
          cyan: "#00e5ff",
          cyan2: "#00bcd4",
          danger: "#ef4444",
          warning: "#f59e0b"
        }
      }
    }
  },
  plugins: []
}
