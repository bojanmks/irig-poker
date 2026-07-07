// BaseLayout.tsx
import { useMemo } from "react"
import { Link, Outlet } from "react-router-dom"

import clsx from "clsx"
import { GitBranch } from "lucide-react"

import logo from "@/assets/logo.png"
import MuteToggle from "@/features/audio/components/MuteToggle"
import LanguageSwitcher from "@/features/localization/components/LanguageSwitcher"
import { buttonVariants } from "@/features/shared/components/shadcn/Button"
import { useAppSelector } from "@/features/store/hooks"
import ThemeSwitcher from "@/features/themes/components/ThemeSwitcher"

const Logo = () => (
  <Link to="/" className="flex items-center">
    <img src={logo} alt="Logo" className="h-10 lg:h-14 w-auto" />
  </Link>
)

const BaseLayout = () => {
  const additionalClass = useAppSelector((state) => state.wrapperClass.additionalClass)

  const baseClass = useMemo(() => "mx-auto p-6 border rounded-2xl shadow-lg bg-background", []);
  const finalClass = useMemo(() => clsx(baseClass, additionalClass), [baseClass, additionalClass]);

  return (
    <div>
      <div className="py-4 px-4 flex justify-between items-center gap-2">
        <Logo />
        <div className="flex items-center gap-2">
          <a 
            href="https://github.com/bojanmks/irig-poker" 
            target="_blank" 
            rel="noopener noreferrer"
            className={buttonVariants({ variant: "outline", size: "icon" })}
          >
            <GitBranch size={16} />
          </a>
          <MuteToggle />
          <ThemeSwitcher />
          <LanguageSwitcher />
        </div>
      </div>

      <div className="p-2">
        <div className={finalClass}>
          <Outlet />
        </div>
      </div>
    </div>
  )
}

export default BaseLayout
