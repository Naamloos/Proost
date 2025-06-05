// eslint.config.js

// @ts-check
import eslintJs from "@eslint/js";
import eslintReact from "@eslint-react/eslint-plugin";
import tseslint from "typescript-eslint";
import eslintPrettier from "eslint-plugin-prettier/recommended";

export default tseslint.config({
  files: ["**/*.ts", "**/*.tsx"],

  // Extend recommended rule sets from:
  // 1. ESLint JS's recommended rules
  // 2. TypeScript ESLint recommended rules
  // 3. ESLint React's recommended-typescript rules
  extends: [
    eslintJs.configs.recommended,
    tseslint.configs.recommended,
    eslintReact.configs["recommended-typescript"],
    eslintPrettier
  ],

  // Configure language/parsing options
  languageOptions: {
    // Use TypeScript ESLint parser for TypeScript files
    parser: tseslint.parser,
    parserOptions: {
      // Enable project service for better TypeScript integration
      projectService: true,
      tsconfigRootDir: import.meta.dirname,
    },
  },

  // Custom rule overrides (modify rule levels or disable rules)
  rules: {
    "@eslint-react/no-missing-key": "warn",
    "@typescript-eslint/no-explicit-any": "off",
    "@eslint-react/hooks-extra/no-direct-set-state-in-use-effect": "off",
    "@eslint-react/no-array-index-key": "off",
  },
});