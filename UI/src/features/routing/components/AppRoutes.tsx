import { Route } from "react-router-dom";

import { CreateGamePage } from "@/features/createGame/pages/CreateGamePage";
import GamePage from "@/features/game/pages/GamePage";
import NotFoundPage from "@/features/routing/components/NotFoundPage";
import BaseLayout from "@/features/shared/layouts/BaseLayout";

import LanguageGuard from "./LanguageGuard";
import LanguageRedirect from "./LanguageRedirect";

const AppRoutes = (
  <>
    <Route path="" element={<LanguageRedirect />} />
    <Route path=":lang" element={<LanguageGuard />}>
      <Route element={<BaseLayout />}>
        <Route path="" element={<CreateGamePage />} />
        <Route path=":gameCode" element={<GamePage />} />
      </Route>
    </Route>
    <Route path="*" element={<NotFoundPage />} />
  </>
);

export default AppRoutes;
