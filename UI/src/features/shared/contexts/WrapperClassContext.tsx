// WrapperClassContext.tsx
import { createContext, useContext, useState } from "react"
import type { ReactNode } from "react"

type WrapperClassContextType = {
  additionalClass: string
  setAdditionalClass: (name: string) => void
}

const WrapperClassContext = createContext<WrapperClassContextType | undefined>(undefined)

export const useWrapperClass = () => {
  const context = useContext(WrapperClassContext)
  if (!context) throw new Error("useWrapperClass must be used within WrapperClassProvider")
  return context
}

export const WrapperClassProvider = ({ children }: { children: ReactNode }) => {
  const [additionalClass, setAdditionalClass] = useState("")

  return (
    <WrapperClassContext.Provider value={{ additionalClass, setAdditionalClass }}>
      {children}
    </WrapperClassContext.Provider>
  )
}
