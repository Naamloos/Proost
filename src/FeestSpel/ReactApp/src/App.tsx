import "./bootstrap";
import { createRoot } from "react-dom/client";
import { createInertiaApp } from "@inertiajs/react";
import { resolvePageComponent } from "laravel-vite-plugin/inertia-helpers";
import "./app.css";

/* @ts-expect-error env DOES exist. */
const appTitle = import.meta.env.VITE_APP_TITLE;

createInertiaApp({
  title: (title) => (title ? `${title} - ${appTitle}` : appTitle),
  resolve: (name) =>
    resolvePageComponent(
      `./Pages/${name}.tsx`,
      /* @ts-expect-error glob pattern is valid. */
      import.meta.glob("./Pages/**/*.{jsx,tsx}"),
    ),

  setup({ el, App, props }) {
    const root = createRoot(el);
    root.render(<App {...props} />);
  },
});
