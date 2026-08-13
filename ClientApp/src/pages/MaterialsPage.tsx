import { useState } from "react";
import { useMaterials } from "../hooks/useMaterials";
import TacticalButton from "../components/TacticalButton";
import MaterialsTable from "../components/materials/MaterialsTable";
import AddMaterialModal from "../components/materials/AddMaterialModal";

export default function MaterialsPage() {
  const { materials, addMaterial } = useMaterials();
  const [showModal, setShowModal] = useState(false);

  if (materials.isLoading) return <div>Loading...</div>;
  if (materials.isError) return <div>Error loading materials.</div>;

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Materials</h1>
        <TacticalButton onClick={() => setShowModal(true)}>
          Add Material
        </TacticalButton>
      </div>

      <MaterialsTable materials={materials.data} />

      {showModal && (
        <AddMaterialModal
          onClose={() => setShowModal(false)}
          onSubmit={(dto) => {
            addMaterial.mutate(dto);
            setShowModal(false);
          }}
        />
      )}
    </div>
  );
}
