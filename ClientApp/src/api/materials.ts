import { api } from "./axios";
import type { NewMaterialDto } from "../types/material";

export const getMaterials = () =>
  api.get("/materials").then(r => r.data);

export const createMaterial = (dto: NewMaterialDto) =>
  api.post("/materials", dto).then(r => r.data);
