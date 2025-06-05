import React, { useState, useEffect } from "react";
import { Beer, QrCode } from "lucide-react";
import { motion, AnimatePresence } from "framer-motion";
import { router } from "@inertiajs/react";

export default function InGame() {
  const [currentChallenge, setCurrentChallenge] = useState("test0");
  const [isHostView, setIsHostView] = useState(false);
  const [isEnding, setIsEnding] = useState(false);

  // Simulating challenge changes
  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentChallenge("test" + Math.floor(Math.random() * 1000));
    }, 3500);

    return () => clearInterval(interval);
  }, []);

  // End game after 15 seconds
  useEffect(() => {
    const timeout = setTimeout(() => {
      setIsEnding(true);
      setTimeout(() => {
        router.get("/beta/end");
      }, 2000); // Delay the redirect to allow fade-out
    }, 15000);

    return () => clearTimeout(timeout);
  }, []);

  // Toggle between player and host view
  const toggleView = () => {
    setIsHostView(!isHostView);
  };

  return (
    <motion.div
      className={`min-h-screen flex flex-col items-center justify-center p-4 relative text-white ${
        isEnding
          ? "bg-app-dark"
          : "bg-gradient-to-br from-purple-800 to-purple-600"
      }`}
      animate={{
        backgroundColor: isEnding ? "rgb(18,18,18)" : ["#6b21a8", "#9333ea"], // Match tailwind gradient
      }}
      transition={{ duration: 2, type: "tween" }}
    >
      {isHostView ? (
        // Host view
        <div className="relative z-10 w-full max-w-md text-center">
          <button
            type="button"
            onClick={toggleView}
            className="absolute top-4 right-4 text-white/80 hover:text-white"
          >
            <span className="bg-black/30 px-3 py-1 rounded-full text-sm">
              Host Client
            </span>
          </button>

          <Beer
            size={40}
            className="mx-auto mb-6 text-beer-amber"
            fill="#FFBF00"
          />

          <AnimatePresence mode="wait">
            <motion.div
              key={currentChallenge}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -20 }}
              transition={{ duration: 0.5 }}
              className="mb-10"
            >
              <h1 className="text-4xl md:text-5xl font-bold leading-tight text-white text-shadow mb-6">
                {currentChallenge}
              </h1>
            </motion.div>
          </AnimatePresence>

          <div className="absolute bottom-8 left-0 right-0 flex justify-between px-8">
            <div className="bg-black/30 px-3 py-1 rounded-full text-sm">
              <span className="text-white/80">Code: 123456</span>
            </div>
            <div className="bg-black/30 p-1 rounded-md">
              <QrCode size={24} className="text-white" />
            </div>
          </div>
        </div>
      ) : (
        // Player view
        <div className="relative z-10 w-full max-w-md text-center">
          <button
            type="button"
            onClick={toggleView}
            className="absolute top-4 right-4 text-white/80 hover:text-white"
          >
            <Beer size={24} className="text-beer-amber" fill="#FFBF00" />
          </button>

          <AnimatePresence mode="wait">
            <motion.div
              key={currentChallenge}
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0, scale: 1.1 }}
              transition={{ duration: 0.5 }}
            >
              <h1 className="text-4xl md:text-5xl font-bold leading-tight text-white text-shadow">
                {currentChallenge}
              </h1>
            </motion.div>
          </AnimatePresence>
        </div>
      )}
    </motion.div>
  );
}
