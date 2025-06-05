import React, { useState } from "react";
import { Beer, Plus, Minus } from "lucide-react";
import Button from "@/Components/Button";
import { router } from "@inertiajs/react";
import BeerBackground from "../Components/BeerBackground";

export default function CreateNew() {
  const [players, setPlayers] = useState([
    { id: "1", name: "" },
    { id: "2", name: "" },
    { id: "3", name: "" },
  ]);
  const [difficulty, setDifficulty] = useState<"easy" | "medium" | "hard">(
    "medium",
  );
  const [packageCount, setPackageCount] = useState(10);

  const addPlayer = () => {
    if (players.length < 8) {
      const newId = (players.length + 1).toString();
      setPlayers([...players, { id: newId, name: "" }]);
    }
  };

  const removePlayer = (id: string) => {
    if (players.length > 3) {
      setPlayers(players.filter((player) => player.id !== id));
    }
  };

  const updatePlayerName = (id: string, name: string) => {
    setPlayers(
      players.map((player) =>
        player.id === id ? { ...player, name } : player,
      ),
    );
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    router.get(`/beta/game`);
  };

  return (
    <div className="min-h-screen bg-app-dark flex flex-col items-center justify-center p-4">
      <BeerBackground />
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <Beer size={36} className="mx-auto text-beer-amber" fill="#FFBF00" />
          <h1 className="text-2xl font-bold mt-2">Nieuw Spel Starten</h1>
        </div>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <h2 className="text-lg font-medium mb-3">Spelers</h2>
            <div className="space-y-2">
              {players.map((player, index) => (
                <div key={player.id} className="flex items-center gap-2">
                  <div className="flex-1">
                    <input
                      type="text"
                      placeholder={`Speler ${index + 1}`}
                      value={player.name}
                      onChange={(e) =>
                        updatePlayerName(player.id, e.target.value)
                      }
                      className="input-field"
                      required
                    />
                  </div>
                  {players.length > 3 && (
                    <button
                      type="button"
                      onClick={() => removePlayer(player.id)}
                      className="text-white/70 hover:text-white p-1"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="20"
                        height="20"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      >
                        <line x1="18" y1="6" x2="6" y2="18" />
                        <line x1="6" y1="6" x2="18" y2="18" />
                      </svg>
                    </button>
                  )}
                </div>
              ))}
            </div>
            {players.length < 8 && (
              <button
                type="button"
                onClick={addPlayer}
                className="mt-2 flex items-center gap-1 text-beer-amber hover:text-beer-gold"
              >
                <Plus size={16} />
                <span>Voeg speler toe</span>
              </button>
            )}
          </div>

          <div>
            <h2 className="text-lg font-medium mb-3">Moeilijkheid</h2>
            <div className="space-y-2">
              <label className="flex items-center gap-2">
                <input
                  type="radio"
                  name="difficulty"
                  checked={difficulty === "easy"}
                  onChange={() => setDifficulty("easy")}
                  className="accent-beer-amber h-4 w-4"
                />
                <span>Nuchter (Alleen slokjes)</span>
              </label>
              <label className="flex items-center gap-2">
                <input
                  type="radio"
                  name="difficulty"
                  checked={difficulty === "medium"}
                  onChange={() => setDifficulty("medium")}
                  className="accent-beer-amber h-4 w-4"
                />
                <span>Normaal (Willekeurig)</span>
              </label>
              <label className="flex items-center gap-2">
                <input
                  type="radio"
                  name="difficulty"
                  checked={difficulty === "hard"}
                  onChange={() => setDifficulty("hard")}
                  className="accent-beer-amber h-4 w-4"
                />
                <span>Dronken (Glas moet altijd leeg)</span>
              </label>
            </div>
          </div>

          <div>
            <h2 className="text-lg font-medium mb-3">Opties</h2>
            <div className="bg-black/30 border-2 border-white/20 rounded-md p-3">
              <div className="flex justify-between items-center">
                <span>Opdrachten</span>
                <div className="flex items-center gap-2">
                  <button
                    type="button"
                    onClick={() =>
                      setPackageCount(Math.max(5, packageCount - 5))
                    }
                    className="bg-white/10 hover:bg-white/20 p-1 rounded"
                  >
                    <Minus size={16} />
                  </button>
                  <span className="w-8 text-center">{packageCount}</span>
                  <button
                    type="button"
                    onClick={() =>
                      setPackageCount(Math.min(50, packageCount + 5))
                    }
                    className="bg-white/10 hover:bg-white/20 p-1 rounded"
                  >
                    <Plus size={16} />
                  </button>
                </div>
              </div>
            </div>
          </div>

          <Button type="submit" className="w-full">
            Start
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
