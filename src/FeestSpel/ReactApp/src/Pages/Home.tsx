import { Head, router } from "@inertiajs/react";
import Button from "../Components/Button";
import Github from "@/Brand/github.svg?react";
import Instagram from "@/Brand/instagram.svg?react";
import BeerBackground from "../Components/BeerBackground";
import { Beer } from "lucide-react";
import { PagePropsWith } from "../Types/PageProps";

interface HomeProps {
  activeGames: number;
  activeClients: number;
}

export default function Home({
  activeGames,
  activeClients,
}: PagePropsWith<HomeProps>) {
  return (
    <>
      <Head title="Proost!" />
      <div className="beer-bg min-h-screen flex flex-col items-center justify-center p-4 relative">
        <BeerBackground />

        <div className="relative z-10 w-full max-w-md text-center">
          <div className="mb-6">
            <Beer
              size={48}
              className="mx-auto text-beer-amber"
              fill="#FFBF00"
            />
          </div>

          <h1 className="text-4xl font-bold mb-2 text-shadow">
            Welkom bij de ProostApp!
          </h1>
          <p className="text-lg mb-8 text-beer-foam text-shadow">
            Drink met mate(n)!
          </p>

          <div className="space-y-4">
            <Button onClick={() => router.get("/beta/new")} className="w-full">
              Nieuwe Sessie
            </Button>

            <Button
              variant="secondary"
              onClick={() => router.get("/beta/join")}
              className="w-full"
            >
              Bestaande Sessie
            </Button>
          </div>

          <div className="mt-8 text-sm text-white/60">
            <p>
              Actieve games: {activeGames} · Actieve displays: {activeClients}
            </p>
          </div>

          <div className="mt-6 flex justify-center space-x-4">
            <a
              href="#"
              className="text-white/70 hover:text-white transition-colors"
            >
              <Github />
            </a>
            <a
              href="#"
              className="text-white/70 hover:text-white transition-colors"
            >
              <Instagram />
            </a>
          </div>

          <div className="mt-12 text-xs text-white/40">
            Deze website gebruikt alleen essentiële cookies.
          </div>
        </div>
      </div>
    </>
  );
}
