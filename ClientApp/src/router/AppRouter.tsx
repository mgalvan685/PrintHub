import { Routes, Route } from "react-router-dom";
import FilamentsPage from "../pages/FilamentsPage";
import MaterialsPage from "../pages/MaterialsPage";
import PrintersPage from "../pages/PrintersPage";

export default function AppRouter() {
  return (
    <Routes>
      <Route path="/filaments" element={<FilamentsPage />} />
      <Route path="/materials" element={<MaterialsPage />} />
      <Route path="/printers" element={<PrintersPage />} />
    </Routes>
  );
}
