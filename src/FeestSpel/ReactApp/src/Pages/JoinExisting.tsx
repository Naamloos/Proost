import React, { useState } from "react";
import { Beer } from "lucide-react";
import Button from "../Components/Button";
import { router } from "@inertiajs/react";
import BeerBackground from "../Components/BeerBackground";

export default function JoinExisting() {
  const [gameCode, setGameCode] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // In a real app, you would validate the game code
    // For this mock version, we'll just accept any code
    router.get("/beta/game");
  };

  return (
    <div className="min-h-screen bg-app-dark flex flex-col items-center justify-center p-4">
      <BeerBackground />
      <div className="w-full max-w-md text-center">
        <Beer size={36} className="mx-auto text-beer-amber" fill="#FFBF00" />
        <h1 className="text-2xl font-bold mt-2 mb-8">Bestaand spel</h1>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <p className="text-white/80 mb-3">Voer hier je room code in.</p>
            <input
              type="text"
              value={gameCode}
              onChange={(e) => setGameCode(e.target.value.toUpperCase())}
              className="input-field text-center text-xl uppercase tracking-wider"
              placeholder="ROOMCODE"
              required
              maxLength={6}
            />
          </div>

          <Button
            type="submit"
            className="w-full"
            disabled={gameCode.length < 6}
          >
            Deelnemen
          </Button>

          <div className="text-center">
            <button
              type="button"
              onClick={() => router.get("/beta")}
              className="text-white/70 hover:text-white text-sm"
            >
              Terug naar home
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
