import React from "react";
import { Beer } from "lucide-react";
import { motion } from "framer-motion";

interface BeerMugProps {
  size?: number;
  position: {
    top: string;
    left: string;
  };
  delay?: number;
  duration?: number;
}

const BeerMug: React.FC<BeerMugProps> = ({
  size = 24,
  position,
  delay = 0,
  duration = 15,
}) => {
  return (
    <motion.div
      className="absolute z-0"
      style={{
        top: position.top,
        left: position.left,
      }}
      initial={{ y: 0, rotate: 0, opacity: 0.7 }}
      animate={{
        y: [0, -10, 5, -5, 0],
        rotate: [0, 5, -5, 2, 0],
        opacity: [0.3, 0.5, 0.3, 0.6, 0.5],
      }}
      transition={{
        duration,
        delay,
        repeat: Infinity,
        repeatType: "loop",
        ease: "easeInOut",
      }}
    >
      <Beer size={size} color="#FFD700" fill="#FFBF00" />
    </motion.div>
  );
};

export default BeerMug;
