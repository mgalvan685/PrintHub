import { useState } from "react";
import TacticalCard from "../TacticalCard";
import TacticalButton from "../TacticalButton";
import TacticalInput from "../TacticalInput";
import type { NewMaterialDto } from "../../types/material";

export default function AddMaterialModal({
  onClose,
  onSubmit
}: {
  onClose: () => void;
  onSubmit: (dto: NewMaterialDto) => void;
}) {
  const [name, setName] = useState("");
  const [initialCost, setInitialCost] = useState("");
  const [units, setUnits] = useState("");
  const [totalMaterial, setTotalMaterial] = useState("");
  const [source, setSource] = useState("");

  return (
    <div className="fixed inset-0 bg-black bg-opacity-60 flex items-center justify-center">
      <TacticalCard>
        <h2 className="text-xl font-bold mb-4">Add Material</h2>

        <TacticalInput label="Name" value={name} onChange={setName} />
        <TacticalInput label="Initial Cost" value={initialCost} onChange={setInitialCost} type="number" />
        <TacticalInput label="Units" value={units} onChange={setUnits} />
        <TacticalInput label="Total Material" value={totalMaterial} onChange={setTotalMaterial} type="number" />
        <TacticalInput label="Source (optional)" value={source} onChange={setSource} />

        <div className="flex gap-3 mt-4">
          <TacticalButton
            onClick={() =>
              onSubmit({
                name,
                initial_Cost: parseFloat(initialCost),
                units,
                total_Material: parseFloat(totalMaterial),
                cost_Per_Unit: parseFloat(initialCost) / parseFloat(totalMaterial),
                source: source || null
              })
            }
          >
            Save
          </TacticalButton>

          <button
            onClick={onClose}
            className="text-tactical-text2 hover:text-tactical-cyan"
          >
            Cancel
          </button>
        </div>
      </TacticalCard>
    </div>
  );
}
