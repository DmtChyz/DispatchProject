import axios from "axios";
import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";

import { authApi } from "./api/api";
import type {
  AuthCredentials,
  AuthUser,
} from "../../types/auth.types";

type AuthContextValue = {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: AuthCredentials) => Promise<void>;
  register: (credentials: AuthCredentials) => Promise<void>;
  logout: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | null>(null);

type AuthProviderProps = {
  children: ReactNode;
};

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    let isCancelled = false;

    async function loadCurrentUser() {
      try {
        const response = await authApi.getCurrentUser();

        if (!isCancelled) {
          setUser(response.data?.user ?? null);
        }
      } catch (error) {
        if (!isCancelled) {
          setUser(null);
        }

        const isUnauthorized =
          axios.isAxiosError(error) &&
          error.response?.status === 401;

        if (!isUnauthorized) {
          console.error("Failed to load the current user.", error);
        }
      } finally {
        if (!isCancelled) {
          setIsLoading(false);
        }
      }
    }

    void loadCurrentUser();

    return () => {
      isCancelled = true;
    };
  }, []);

  const login = useCallback(async (credentials: AuthCredentials) => {
    const response = await authApi.login(credentials);
    const authenticatedUser = response.data?.user;

    if (!authenticatedUser) {
      throw new Error("Login response did not contain user data.");
    }

    setUser(authenticatedUser);
  }, []);

  const register = useCallback(async (credentials: AuthCredentials) => {
    const response = await authApi.register(credentials);
    const authenticatedUser = response.data?.user;

    if (!authenticatedUser) {
      throw new Error("Registration response did not contain user data.");
    }

    setUser(authenticatedUser);
  }, []);

  const logout = useCallback(async () => {
    try {
      await authApi.logout();
      setUser(null);
    } catch (error) {
      const isUnauthorized =
        axios.isAxiosError(error) &&
        error.response?.status === 401;

      if (isUnauthorized) {
        setUser(null);
        return;
      }

      throw error;
    }
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: user !== null,
      isLoading,
      login,
      register,
      logout,
    }),
    [user, isLoading, login, register, logout]
  );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  if (context === null) {
    throw new Error("useAuth must be used inside AuthProvider.");
  }

  return context;
}