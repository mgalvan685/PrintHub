import { useQuery, useMutation } from "@tanstack/react-query";
import { getMaterials, createMaterial } from "../api/materials";

export const useMaterials = () => {
  const materials = useQuery({
    queryKey: ["materials"],
    queryFn: getMaterials
  });

  const addMaterial = useMutation({
    mutationFn: createMaterial,
    onSuccess: () => materials.refetch()
  });

  return { materials, addMaterial };
};
