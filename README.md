# 🚀 Rover Mission Planner

Sistema de planificación y visualización de tareas diarias para rovers exploradores en Marte. Desarrollo como parte de una prueba técnica para el cargo de **Desarrollador Fullstack**

---

## 📂 Estructura del proyecto

```
rover-mission-planner/
├── backend/ → API REST en .NET 8 con arquitectura en capas
├── frontend/ → SPA Angular 18 para visualización de timeline
└── .github/workflows → Pipelines de CI con GitHub Actions
```

---

## 🧠 Tecnologías utilizadas

- **.NET 8** (Web API)
- **Arquitectura limpia**: Domain, Application, API, Tests
- **FluentValidation** para reglas de negocio
- **xUnit** + **FluentAssertions** para testing legible
- **Coverlet** para cobertura de código
- **Angular 18** (standalone)
- **PrimeNG** para componente de timeline
- **Docker** para contenedor de la API
- **GitHub Actions** para integración continua (CI)

---

## ⚙️ Instrucciones rápidas

### ▶️ Backend (.NET 8)

Desde la carpeta `backend`:

```bash
# 1. Restaurar paquetes y compilar
dotnet build

# 2. Ejecutar el proyecto
dotnet run --project RoverMissionPlanner.API

# 3. Ejecutar tests
dotnet test --collect:"XPlat Code Coverage"
```

> ℹ️ La API corre por defecto en http://localhost:5225.

### 🐳 Docker (API backend)

```bash
# Build de imagen
docker build -t rover-api .\backend\RoverMissionPlanner\

# Ejecutar contenedor
docker run -p 5225:5225 --name rover-api-container rover-api
```

> Accede a Swagger: http://localhost:5225/swagger/index.html

### 🖼️ Frontend (Angular 18)

Desde la carpeta frontend:

```bash
# 1. Instalar dependencias
npm install

# 2. Levantar en modo desarrollo
npm run start
```

> Accede desde http://localhost:4200.

---

## 🧪 Cobertura de pruebas

- Cobertura mínima exigida en lógica crítica: 70%
- Cobertura actual: >90% en servicios críticos

Para generar reporte HTML:

```bash
dotnet test backend/RoverMissionPlanner/RoverMissionPlanner.Tests /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=coverage/lcov.info
```

---

## 🧪 Endpoints principales

| **Método** | **Endpoint**                               | **Descripción**                                    |
| ---------- | ------------------------------------------ | -------------------------------------------------- |
| POST       | `/rovers/{id}/tasks`                       | Crear tarea para un rover, validando solapamientos |
| GET        | `/rovers/{id}/tasks?date=YYYY-MM-DD`       | Listar tareas de un día para el rover              |
| GET        | `/rovers/{id}/utilization?date=YYYY-MM-DD` | Porcentaje de utilización del rover en ese día     |

---

## 🧠 Reglas de negocio clave

- Tareas no deben solaparse en tiempo para un mismo rover.
- Validación de duración, coordenadas y fechas con FluentValidation.
- Uso de 1440 minutos para calcular porcentaje diario de utilización.

---

## 📦 CI y Calidad

- CI automático con GitHub Actions (build + test)
- Pruebas unitarias con xUnit
- Aserciones legibles con FluentAssertions
- Validaciones estrictas en DTOs con FluentValidation

---

## 🏁 Entrega

- Repositorio público: https://github.com/JonathanNavarroV/rover-mission-planner
- Tag de entrega: v1.0

---

## ✍️ Autor

- [Jonathan Navarro](https://github.com/JonathanNavarroV)
- 📧 jonathan.d.navarro.v@gmail.com
