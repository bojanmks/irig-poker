import { useState } from "react";
import { useLocation } from "react-router-dom";

export function useUsernameFromRoute() {
  const location = useLocation();
  const [username, setUsername] = useState<string | undefined>(location.state?.username);

  return { username, setUsername };
}
