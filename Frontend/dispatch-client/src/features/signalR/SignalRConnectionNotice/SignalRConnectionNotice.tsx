import { useEffect, useState } from "react";

import { StatusDot } from "../../../components/animated/StatusDot/StatusDot";
import { TRANSLATION_KEYS } from "../../../constants/translationKeys";
import { useTranslation } from "../../../hooks/useTranslations";
import { useSignalR } from "../SignalRProvider";
import styles from "./SignalRConnectionNotice.module.css";

const NOTICE_DURATION_MS = 4000;

export function SignalRConnectionNotice() {
  const { status } = useSignalR();
  const { t } = useTranslation();

  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    if (status !== "connected") {
      setIsVisible(false);
      return;
    }

    setIsVisible(true);

    const timeout = setTimeout(() => {
      setIsVisible(false);
    }, NOTICE_DURATION_MS);

    return () => {
      clearTimeout(timeout);
    };
  }, [status]);

  if (!isVisible) {
    return null;
  }

  return (
    <aside
      className={styles.notice}
      role="status"
      aria-live="polite"
    >
      <StatusDot
        isActive
        size="sm"
        pulse
      />

      <span className={styles.message}>
        {t(TRANSLATION_KEYS.signalR.connected)}
      </span>
    </aside>
  );
}