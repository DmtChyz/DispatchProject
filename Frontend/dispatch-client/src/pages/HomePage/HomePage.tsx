import { useState } from "react";
import { useNavigate } from "react-router-dom";

import { Button } from "../../components/ui/Button/Button";
import { Card } from "../../components/ui/Card/Card";
import { TRANSLATION_KEYS } from "../../constants/translationKeys";
import { useAuth } from "../../features/auth/AuthProvider";
import { AuthModal } from "../../features/auth/components/authModal/AuthModal";
import { useTranslation } from "../../hooks/useTranslations";
import type { AuthMode } from "../../types/auth.types";
import styles from "./HomePage.module.css";

export function HomePage() {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const { isAuthenticated } = useAuth();

  const [authMode, setAuthMode] = useState<AuthMode | null>(null);

  function handlePrimaryAction() {
    if (isAuthenticated) {
      navigate("/dashboard");
      return;
    }

    setAuthMode("register");
  }

  return (
    <>
      <main className={styles.page}>
        <section className={styles.hero}>
          <div className={styles.heroContent}>
            <span className={styles.eyebrow}>
              {t(TRANSLATION_KEYS.home.eyebrow)}
            </span>

            <h1 className={styles.title}>
              {t(TRANSLATION_KEYS.home.title)}
            </h1>

            <p className={styles.description}>
              {t(TRANSLATION_KEYS.home.description)}
            </p>

            <div className={styles.heroActions}>
              <Button size="lg" onClick={handlePrimaryAction}>
                {isAuthenticated
                  ? t(TRANSLATION_KEYS.home.openDashboard)
                  : t(TRANSLATION_KEYS.home.getStarted)}
              </Button>

              {!isAuthenticated && (
                <Button
                  size="lg"
                  variant="secondary"
                  onClick={() => setAuthMode("login")}
                >
                  {t(TRANSLATION_KEYS.navigation.login)}
                </Button>
              )}
            </div>
          </div>

          <Card className={styles.previewCard}>
            <div className={styles.previewHeader}>
              <div>
                <span className={styles.previewEyebrow}>
                  {t(TRANSLATION_KEYS.home.previewEyebrow)}
                </span>

                <h2 className={styles.previewTitle}>
                  {t(TRANSLATION_KEYS.home.previewTitle)}
                </h2>
              </div>

              <span className={styles.liveBadge}>
                <span className={styles.liveDot} />
                {t(TRANSLATION_KEYS.home.live)}
              </span>
            </div>

            <div className={styles.channelList}>
              <div className={styles.channelItem}>
                <div>
                  <strong>{t(TRANSLATION_KEYS.home.email)}</strong>
                  <span>
                    {t(TRANSLATION_KEYS.home.emailDescription)}
                  </span>
                </div>

                <span className={styles.channelStatus}>
                  {t(TRANSLATION_KEYS.home.ready)}
                </span>
              </div>

              <div className={styles.channelItem}>
                <div>
                  <strong>{t(TRANSLATION_KEYS.home.sms)}</strong>
                  <span>
                    {t(TRANSLATION_KEYS.home.smsDescription)}
                  </span>
                </div>

                <span className={styles.channelStatus}>
                  {t(TRANSLATION_KEYS.home.ready)}
                </span>
              </div>

              <div className={styles.channelItem}>
                <div>
                  <strong>{t(TRANSLATION_KEYS.home.tracking)}</strong>
                  <span>
                    {t(TRANSLATION_KEYS.home.trackingDescription)}
                  </span>
                </div>

                <span className={styles.channelStatus}>
                  {t(TRANSLATION_KEYS.home.connected)}
                </span>
              </div>
            </div>
          </Card>
        </section>

        <section className={styles.features}>
          <div className={styles.sectionHeader}>
            <span className={styles.eyebrow}>
              {t(TRANSLATION_KEYS.home.featuresEyebrow)}
            </span>

            <h2 className={styles.sectionTitle}>
              {t(TRANSLATION_KEYS.home.featuresTitle)}
            </h2>

            <p className={styles.sectionDescription}>
              {t(TRANSLATION_KEYS.home.featuresDescription)}
            </p>
          </div>

          <div className={styles.featureGrid}>
            <Card
              className={styles.featureCard}
              title={t(TRANSLATION_KEYS.home.sendTitle)}
              description={t(TRANSLATION_KEYS.home.sendDescription)}
            >
              <span className={styles.featureNumber}>01</span>
            </Card>

            <Card
              className={styles.featureCard}
              title={t(TRANSLATION_KEYS.home.trackTitle)}
              description={t(TRANSLATION_KEYS.home.trackDescription)}
            >
              <span className={styles.featureNumber}>02</span>
            </Card>

            <Card
              className={styles.featureCard}
              title={t(TRANSLATION_KEYS.home.historyTitle)}
              description={t(TRANSLATION_KEYS.home.historyDescription)}
            >
              <span className={styles.featureNumber}>03</span>
            </Card>
          </div>
        </section>

        <section className={styles.cta}>
          <div>
            <h2 className={styles.ctaTitle}>
              {t(TRANSLATION_KEYS.home.ctaTitle)}
            </h2>

            <p className={styles.ctaDescription}>
              {t(TRANSLATION_KEYS.home.ctaDescription)}
            </p>
          </div>

          <Button size="lg" onClick={handlePrimaryAction}>
            {isAuthenticated
              ? t(TRANSLATION_KEYS.home.openDashboard)
              : t(TRANSLATION_KEYS.home.getStarted)}
          </Button>
        </section>
      </main>

      <AuthModal
        isOpen={authMode !== null}
        initialMode={authMode ?? "login"}
        onClose={() => setAuthMode(null)}
      />
    </>
  );
}