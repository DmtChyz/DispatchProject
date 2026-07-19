import { createBrowserRouter } from "react-router-dom";

import { ProtectedRoute } from "./features/auth/components/ProtectedRoute/ProtectedRoute";
import { MainLayout } from "./layouts/MainLayout";
import { DashboardPage } from "./pages/DashboardPage/DashboardPage";
import { HomePage } from "./pages/HomePage/HomePage";

export const router = createBrowserRouter([
  {
    element: <MainLayout />,
    children: [
      {
        path: "/",
        element: <HomePage />,
      },
      {
        path: "/dashboard",
        element: (
          <ProtectedRoute>
            <DashboardPage />
          </ProtectedRoute>
        ),
      },
    ],
  },
]);