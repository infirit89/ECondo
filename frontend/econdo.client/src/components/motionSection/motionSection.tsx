import { useInViewport } from "@mantine/hooks";
import { motion } from "framer-motion";
import { ReactNode } from "react";

export function MotionSection({ children }: { children: ReactNode }) {
    const { ref, inViewport } = useInViewport();
  
    return (
      <div ref={ref}>
        <motion.div
          initial={{ opacity: 0, y: 50 }}
          animate={inViewport ? { opacity: 1, y: 0 } : { opacity: 0, y: 50 }}
          transition={{ duration: 0.6, type: 'spring', bounce: 0.3 }}
        >
          {children}
        </motion.div>
      </div>
    );
  }