import { Outlet } from "react-router-dom";

import { Header } from "../components/navigation/Header/Header";
import styles from "./MainLayout.module.css";

export function MainLayout() {
  return (
    <>
      <Header />

      <div className={styles.main}>
        <Outlet />
      </div>
    </>
  );
}