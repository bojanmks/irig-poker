// BaseLayout.tsx
import LanguageSwitcher from "@/features/localization/components/LanguageSwitcher"
import ThemeSwitcher from "@/features/themes/components/ThemeSwitcher"
import { Outlet } from "react-router-dom"
import clsx from "clsx"
import { useWrapperClass, WrapperClassProvider } from "../contexts/WrapperClassContext"
import { useMemo } from "react"
import { Github } from "lucide-react"

const LayoutContent = () => {
  const { additionalClass } = useWrapperClass()

  const baseClass = useMemo(() => "mx-auto p-6 border rounded-2xl shadow-lg bg-background", []);
  const finalClass = useMemo(() => clsx(baseClass, additionalClass), [baseClass, additionalClass]);

  return (
    <div>
      <div className="py-4 pr-4 flex justify-end items-center gap-2">
        <a 
          href="https://github.com/bojanmks" 
          target="_blank" 
          rel="noopener noreferrer"
          className="flex items-center justify-center w-8 h-8 rounded-md border border-border hover:bg-accent transition-colors"
        >
          <Github size={16} />
        </a>
        <ThemeSwitcher />
        <LanguageSwitcher />
      </div>

      <div className="p-2">
        <div className={finalClass}>
          <Outlet />
        </div>
      </div>
    </div>
  )
}

const BaseLayout = () => (
  <WrapperClassProvider>
    <LayoutContent />
  </WrapperClassProvider>
)

export default BaseLayout
