import { useState } from "react";
import { useFilaments } from "../hooks/useFilaments";
import TacticalButton from "../components/TacticalButton";
import FilamentsTable from "../components/filaments/FilamentsTable";
import AddFilamentModal from "../components/filaments/AddFilamentModal";

export default function FilamentsPage() {
  const { filaments, addFilament } = useFilaments();
  const [showModal, setShowModal] = useState(false);

  if (filaments.isLoading) return <div>Loading...</div>;
  if (filaments.isError) return <div>Error loading filaments.</div>;

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Filaments</h1>
        <TacticalButton onClick={() => setShowModal(true)}>
          Add Filament
        </TacticalButton>
      </div>

      <FilamentsTable filaments={filaments.data} />

      {showModal && (
        <AddFilamentModal
  onClose={() => setShowModal(false)}
  onSubmit={(dto) => {
    addFilament.mutate(dto);
    setShowModal(false);
  }}
/>

      )}
    </div>
  );
}
