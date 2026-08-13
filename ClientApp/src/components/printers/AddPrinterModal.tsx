import { useState } from "react";
import TacticalCard from "../TacticalCard";
import TacticalButton from "../TacticalButton";
import TacticalInput from "../TacticalInput";
import type { NewPrinterDto } from "../../types/printer";

export default function AddPrinterModal({
  onClose,
  onSubmit
}: {
  onClose: () => void;
  onSubmit: (dto: NewPrinterDto) => void;
}) {
  const [brand, setBrand] = useState("");
  const [type, setType] = useState("");
  const [name, setName] = useState("");
  const [power, setPower] = useState("");

  return (
    <div className="fixed inset-0 bg-black bg-opacity-60 flex items-center justify-center">
      <TacticalCard>
        <h2 className="text-xl font-bold mb-4">Add Printer</h2>

        <TacticalInput label="Brand" value={brand} onChange={setBrand} />
        <TacticalInput label="Type" value={type} onChange={setType} />
        <TacticalInput label="Name" value={name} onChange={setName} />
        <TacticalInput
          label="Power Per Hour (kWh)"
          value={power}
          onChange={setPower}
          type="number"
        />

        <div className="flex gap-3 mt-4">
          <TacticalButton
            onClick={() =>
              onSubmit({
                brand,
                type,
                name,
                power_Per_Hour: parseFloat(power)
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
