import { Routes, Route } from "react-router-dom";
import FilamentsPage from "../pages/FilamentsPage";

export default function AppRouter() {
  return (
    <Routes>
      <Route path="/filaments" element={<FilamentsPage />} />
    </Routes>
  );
}
