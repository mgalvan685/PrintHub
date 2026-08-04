import { useQuery, useMutation } from "@tanstack/react-query";
import { getFilaments, createFilament } from "../api/filaments";
import type { NewFilamentDto } from "../types/filament";

export const useFilaments = () => {
  const filaments = useQuery({
    queryKey: ["filaments"],
    queryFn: getFilaments
  });

  const addFilament = useMutation({
    mutationFn: (dto: NewFilamentDto) => createFilament(dto),
    onSuccess: () => filaments.refetch()
  });

  return { filaments, addFilament };
};
