import { lazy, Suspense } from "react";
import { Route } from "react-router-dom";

import { Loader2 } from "lucide-react";

import { CreateGamePage } from "@/features/createGame/pages/CreateGamePage";
import NotFoundPage from "@/features/routing/components/NotFoundPage";
import BaseLayout from "@/features/shared/layouts/BaseLayout";

import LanguageGuard from "./LanguageGuard";
import LanguageRedirect from "./LanguageRedirect";

const GamePage = lazy(() => import("@/features/game/pages/GamePage"));

const GamePageFallback = () => (
  <div className="flex items-center justify-center">
    <Loader2 className="h-8 w-8 animate-spin" />
  </div>
);

const AppRoutes = (
  <>
    <Route path="" element={<LanguageRedirect />} />
    <Route path=":lang" element={<LanguageGuard />}>
      <Route element={<BaseLayout />}>
        <Route path="" element={<CreateGamePage />} />
        <Route path=":gameCode" element={<Suspense fallback={<GamePageFallback />}><GamePage /></Suspense>} />
      </Route>
    </Route>
    <Route path="*" element={<NotFoundPage />} />
  </>
);

export default AppRoutes;
