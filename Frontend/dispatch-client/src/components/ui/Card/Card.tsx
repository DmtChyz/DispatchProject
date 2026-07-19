import type { HTMLAttributes, ReactNode } from "react";
import styles from "./Card.module.css";

type CardProps = HTMLAttributes<HTMLDivElement> & {
  children: ReactNode;
  title?: string;
  description?: string;
  actions?: ReactNode;
};

export function Card({
  children,
  title,
  description,
  actions,
  className,
  ...props
}: CardProps) {
  const cardClassName = [styles.card, className ?? ""]
    .filter(Boolean)
    .join(" ");

  return (
    <section {...props} className={cardClassName}>
      {(title || description || actions) && (
        <header className={styles.header}>
          <div>
            {title && <h2 className={styles.title}>{title}</h2>}
            {description && <p className={styles.description}>{description}</p>}
          </div>

          {actions && <div className={styles.actions}>{actions}</div>}
        </header>
      )}

      <div className={styles.content}>{children}</div>
    </section>
  );
}