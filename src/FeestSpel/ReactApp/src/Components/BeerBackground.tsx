import React, { useRef } from "react";
import BeerMug from "./BeerMug";

export default function BeerBackground() {
  const beerMugsRef = useRef<
    {
      id: number;
      size: number;
      position: {
        top: string;
        left: string;
      };
      delay: number;
      duration: number;
    }[]
  >([]);

  // Generate random positions for beer mugs with spacing
  const generateBeerMugs = (count: number) => {
    if (beerMugsRef.current.length > 0) {
      return beerMugsRef.current;
    }

    const mugs = [];
    const gridSize = 5; // 5x5 grid
    const cellWidth = 100 / gridSize;
    const cellHeight = 100 / gridSize;

    // Create a grid of possible positions
    const positions = [];
    for (let i = 0; i < gridSize; i++) {
      for (let j = 0; j < gridSize; j++) {
        positions.push({
          top: i * cellHeight + Math.random() * (cellHeight * 0.6),
          left: j * cellWidth + Math.random() * (cellWidth * 0.6),
        });
      }
    }

    // Shuffle positions
    for (let i = positions.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [positions[i], positions[j]] = [positions[j], positions[i]];
    }

    // Generate mugs using the shuffled positions
    for (let i = 0; i < Math.min(count, positions.length); i++) {
      const size = Math.floor(Math.random() * 16) + 16; // Random size between 16-32
      mugs.push({
        id: i,
        size,
        position: {
          top: `${positions[i].top}%`,
          left: `${positions[i].left}%`,
        },
        delay: Math.random() * 2,
        duration: Math.random() * 2 + 9, // Between 9-11 seconds
      });
    }

    beerMugsRef.current = mugs;
    return mugs;
  };

  const beerMugs = generateBeerMugs(20);

  return (
    <div className="fixed inset-0 overflow-hidden pointer-events-none z-0">
      {beerMugs.map((mug) => (
        <BeerMug
          key={mug.id}
          size={mug.size}
          position={mug.position}
          delay={mug.delay}
          duration={mug.duration}
        />
      ))}
    </div>
  );
}
