import { useLocation } from "react-router-dom";
import { useState, useEffect } from "react";

export function useUsernameFromRoute() {
  const location = useLocation();
  const [username, setUsername] = useState<string | null>(null);
  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    const routeUsername = location.state?.username;
    if (routeUsername) setUsername(routeUsername);
    setInitialized(true); // Mark as initialized regardless
  }, [location.state?.username, setUsername, setInitialized]);

  return { username, setUsername, initialized };
}
