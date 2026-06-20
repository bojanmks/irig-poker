import { Navigate, Route } from "react-router-dom";

import { CreateGamePage } from "@/features/createGame/pages/CreateGamePage";
import GamePage from "@/features/game/pages/GamePage";
import BaseLayout from "@/features/shared/layouts/BaseLayout";

import type { AppRoute } from "../models/AppRoute";

const routes: AppRoute[] = [
    {
        basePath: "/",
        layoutElement: <BaseLayout />,
        children: [
            {
                path: "/",
                element: <CreateGamePage />
            },
            {
                path: "/:gameCode",
                element: <GamePage />
            }
        ]
    },
];

const AppRoutes = (
    <>
        {routes.map(parentRoute =>
            <Route path={parentRoute.basePath} element={parentRoute.layoutElement} key={parentRoute.basePath}>
                {parentRoute.children?.map(childRoute =>
                    <Route key={childRoute.path}>
                        <Route path={childRoute.path} element={childRoute.element} />
                    </Route>
                )}

                <Route path={parentRoute.basePath} element={<Navigate to='/' />} />
            </Route>
        )}

        <Route path="*" element={<Navigate to='/' />} />
    </>
)

export default AppRoutes;