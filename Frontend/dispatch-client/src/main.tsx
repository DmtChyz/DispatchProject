import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { RouterProvider } from "react-router-dom";

import { router } from "./router";
import { AuthProvider } from "./features/auth/AuthProvider";
import { SettingsProvider } from "./features/settings/SettingsProvider";
import { SignalRProvider } from "./features/signalR/SignalRProvider";
import { SignalRConnectionNotice } from "./features/signalR/SignalRConnectionNotice/SignalRConnectionNotice";

import "./styles/theme.css";
import "./styles/global.css";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <SettingsProvider>
      <AuthProvider>
        <SignalRProvider>
          <SignalRConnectionNotice />
          <RouterProvider router={router} />
        </SignalRProvider>
      </AuthProvider>
    </SettingsProvider>
  </StrictMode>
);