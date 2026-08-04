import { useState } from "react";
import TacticalCard from "../TacticalCard";
import TacticalButton from "../TacticalButton";
import TacticalInput from "../TacticalInput";
import type { NewFilamentDto } from "../../types/filament";

export default function AddFilamentModal({
  onClose,
  onSubmit
}: {
  onClose: () => void;
  onSubmit: (dto: NewFilamentDto) => void;
}) {
  const [brand, setBrand] = useState("");
  const [material, setMaterial] = useState("");
  const [texture, setTexture] = useState("");
  const [color, setColor] = useState("");
  const [cost, setCost] = useState("");

  return (
    <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center">
      <TacticalCard>
        <h2 className="text-xl font-bold mb-4">Add Filament</h2>

        <TacticalInput label="Brand" value={brand} onChange={setBrand} />
        <TacticalInput label="Material" value={material} onChange={setMaterial} />
        <TacticalInput label="Texture" value={texture} onChange={setTexture} />
        <TacticalInput label="Color" value={color} onChange={setColor} />
        <TacticalInput
          label="Cost Per KG"
          value={cost}
          onChange={setCost}
          type="number"
        />

        <div className="flex gap-3 mt-4">
          <TacticalButton
            onClick={() =>
              onSubmit({
                brand,
                material,
                texture,
                color,
                cost: parseFloat(cost)
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
