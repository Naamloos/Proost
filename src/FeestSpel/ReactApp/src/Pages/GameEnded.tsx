import React, { useEffect, useState } from "react";
import { Beer } from "lucide-react";
import { motion } from "framer-motion";
import { router } from "@inertiajs/react";

export default function GameEnded() {
  const [countdown, setCountdown] = useState(3);

  useEffect(() => {
    const timer = setInterval(() => {
      setCountdown((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          // Redirect to home after countdown
          setTimeout(() => router.get("/beta"), 500);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  return (
    <div className="min-h-screen bg-app-dark flex flex-col items-center justify-center p-4">
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
        className="text-center"
      >
        <Beer
          size={40}
          className="mx-auto mb-4 text-beer-amber"
          fill="#FFBF00"
        />
        <h1 className="text-3xl font-bold mb-2">Het spel is voorbij!</h1>
        <p className="text-white/80 mb-4">
          Je wordt terug gestuurd naar de homepage in {countdown} seconden...
        </p>

        <motion.div
          initial={{ scale: 1 }}
          animate={{ scale: [1, 1.2, 1] }}
          transition={{
            duration: 1,
            repeat: Infinity,
            repeatType: "loop",
          }}
        >
          <span className="text-4xl">{countdown}</span>
        </motion.div>
      </motion.div>
    </div>
  );
}
