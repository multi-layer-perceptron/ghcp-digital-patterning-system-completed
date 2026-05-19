# Tasks: Digital Patterning System Simulator

**Input**: Design documents from `specs/001-digital-patterning-simulator/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/`, `quickstart.md`

**Tests**: Included because the quickstart and industrial-stack plan require xUnit, CTest, SQL, TCP/IP, PLC stub, and FPGA stub validation.

**Organization**: Tasks are grouped by user story so each story can be implemented and validated independently after the shared foundation is complete.

## Phase 1: Setup

**Purpose**: Create the cross-language project skeleton for the requested C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLC stack.

- [X] T001 Create C# solution file at `workspace/csharp/PatterningSimulator.sln`
- [X] T002 [P] Create C# dashboard project at `workspace/csharp/PatterningOperatorDashboard/PatterningOperatorDashboard.csproj`
- [X] T003 [P] Create C# core library project at `workspace/csharp/Patterning.Core/Patterning.Core.csproj`
- [X] T004 [P] Create C# infrastructure library project at `workspace/csharp/Patterning.Infrastructure/Patterning.Infrastructure.csproj`
- [X] T005 [P] Create C# xUnit test project at `workspace/csharp/Patterning.Tests/Patterning.Tests.csproj`
- [X] T110 [P] Create C# executable gateway host project at `workspace/csharp/Patterning.GatewayHost/Patterning.GatewayHost.csproj`
- [X] T006 [P] Create C++ pattern processor CMake project at `workspace/cpp/CMakeLists.txt`
- [X] T007 [P] Create C control emulator CMake project at `workspace/control-c/CMakeLists.txt`
- [X] T008 [P] Create SQL migrations folder and README at `workspace/sql/README.md`
- [X] T009 [P] Create PLC stub folder and README at `workspace/plc/README.md`
- [X] T010 [P] Create FPGA stub folder and README at `workspace/fpga/README.md`
- [X] T011 [P] Create generic sample asset manifest at `workspace/assets/samples/manifest.json`

---

## Phase 2: Foundational

**Purpose**: Establish shared models, protocol boundaries, persistence, and validation primitives that block all user stories.

**Critical**: No user story work should begin until this phase is complete.

- [X] T012 Define C# domain enums in `workspace/csharp/Patterning.Core/Models/PatterningEnums.cs`
- [X] T013 [P] Define C# design and metadata records in `workspace/csharp/Patterning.Core/Models/DesignConcept.cs`
- [X] T014 [P] Define C# palette, channel, and mapping records in `workspace/csharp/Patterning.Core/Models/PaletteModels.cs`
- [X] T015 [P] Define C# grid, diagnostic, run, and report records in `workspace/csharp/Patterning.Core/Models/ProductionModels.cs`
- [X] T016 Define TCP/IP envelope and message contracts in `workspace/csharp/Patterning.Core/Protocol/MachineProtocolEnvelope.cs`
- [X] T017 [P] Define C++ protocol structs in `workspace/cpp/include/protocol.hpp`
- [X] T018 [P] Define C control protocol structs in `workspace/control-c/include/patterning_protocol.h`
- [X] T019 Copy SQL schema contract into migration file `workspace/sql/migrations/V1__create_patterning_tables.sql`
- [X] T020 Implement C# SQL connection settings in `workspace/csharp/Patterning.Infrastructure/Data/SqlOptions.cs`
- [X] T021 Implement C# SQL repository base with parameterized command helpers in `workspace/csharp/Patterning.Infrastructure/Data/SqlRepositoryBase.cs`
- [X] T022 Implement C# TCP client abstraction in `workspace/csharp/Patterning.Infrastructure/Tcp/PatterningTcpClient.cs`
- [X] T023 Implement C# confidentiality-safe text validator in `workspace/csharp/Patterning.Core/Validation/ConfidentialitySafeText.cs`
- [X] T024 [P] Implement C++ JSON-lines protocol helpers in `workspace/cpp/src/protocol.cpp`
- [X] T025 [P] Implement C JSON-lines protocol helpers in `workspace/control-c/src/protocol.c`
- [X] T026 Add solution build instructions for all foundational components in `workspace/README.md`

**Checkpoint**: Foundation ready - all user story phases can now begin.

---

## Phase 3: User Story 1 - Upload, Analyze, and Preview a Design (Priority: P1)

**Goal**: A user can upload or select a generic PNG/JPEG design, see a preview, and view metadata plus a 4 to 16 color palette without starting simulation.

**Independent Test**: Run C# dashboard tests and C++ processor tests, select the generic sample or upload a valid PNG/JPEG, and confirm preview, dimensions, aspect ratio, estimated color count, and palette appear while invalid files preserve previous state.

### Tests for User Story 1

- [X] T027 [P] [US1] Add C# upload validation tests in `workspace/csharp/Patterning.Tests/UploadValidationTests.cs`
- [X] T028 [P] [US1] Add C# concept analysis orchestration tests in `workspace/csharp/Patterning.Tests/ConceptAnalysisWorkflowTests.cs`
- [X] T029 [P] [US1] Add C++ image metadata tests in `workspace/cpp/tests/image_metadata_tests.cpp`
- [X] T030 [P] [US1] Add C++ palette extraction tests in `workspace/cpp/tests/palette_extraction_tests.cpp`
- [X] T031 [P] [US1] Add SQL concept and palette repository tests in `workspace/csharp/Patterning.Tests/ConceptRepositoryTests.cs`

### Implementation for User Story 1

- [X] T032 [P] [US1] Implement C++ image input validator in `workspace/cpp/include/image_validator.hpp`
- [X] T033 [US1] Implement C++ image input validator logic in `workspace/cpp/src/image_validator.cpp`
- [X] T034 [P] [US1] Implement C++ metadata extractor interface in `workspace/cpp/include/image_metadata.hpp`
- [X] T035 [US1] Implement C++ metadata extractor logic in `workspace/cpp/src/image_metadata.cpp`
- [X] T036 [P] [US1] Implement C++ palette extractor interface in `workspace/cpp/include/palette_extractor.hpp`
- [X] T037 [US1] Implement deterministic C++ palette extraction in `workspace/cpp/src/palette_extractor.cpp`
- [X] T038 [US1] Implement C++ `concept.analyze` service entry point in `workspace/cpp/src/pattern_processor_main.cpp`
- [X] T039 [US1] Implement C# upload validation service in `workspace/csharp/Patterning.Core/Services/UploadValidationService.cs`
- [X] T040 [US1] Implement C# concept analysis orchestrator in `workspace/csharp/Patterning.Core/Services/ConceptAnalysisService.cs`
- [X] T041 [US1] Implement C# concept SQL repository in `workspace/csharp/Patterning.Infrastructure/Data/ConceptRepository.cs`
- [X] T042 [US1] Implement C# dashboard upload view model in `workspace/csharp/PatterningOperatorDashboard/ViewModels/UploadDesignViewModel.cs`
- [X] T043 [US1] Implement C# dashboard upload view in `workspace/csharp/PatterningOperatorDashboard/Views/UploadDesignView.xaml`
- [X] T044 [US1] Add generic sample design metadata in `workspace/assets/samples/generic-floorcovering-sample.json`
- [X] T112 [P] [US1] Add generic sample PNG image asset in `workspace/assets/samples/generic-floorcovering-sample.png`

**Checkpoint**: User Story 1 is independently functional and demoable.

---

## Phase 4: User Story 2 - Map Colors to Manufacturing Channels (Priority: P2)

**Goal**: A user can review extracted palette colors, edit 8 generic manufacturing channels for the session, and see exact, approximate, or unresolved mappings with a mapped preview.

**Independent Test**: Start from an analyzed sample design, edit channel labels/colors, run mapping, and verify every palette color has a mapping status and the mapped preview plus diagnostics update.

### Tests for User Story 2

- [X] T045 [P] [US2] Add C# channel defaults and editing tests in `workspace/csharp/Patterning.Tests/ManufacturingChannelTests.cs`
- [X] T046 [P] [US2] Add C# channel mapping workflow tests in `workspace/csharp/Patterning.Tests/ChannelMappingWorkflowTests.cs`
- [X] T047 [P] [US2] Add C++ color delta and mapping tests in `workspace/cpp/tests/channel_mapping_tests.cpp`
- [X] T048 [P] [US2] Add SQL channel and mapping repository tests in `workspace/csharp/Patterning.Tests/ChannelMappingRepositoryTests.cs`

### Implementation for User Story 2

- [X] T049 [P] [US2] Implement C# default channel factory in `workspace/csharp/Patterning.Core/Services/DefaultChannelFactory.cs`
- [X] T050 [US2] Implement C# channel editing service in `workspace/csharp/Patterning.Core/Services/ManufacturingChannelService.cs`
- [X] T051 [P] [US2] Implement C++ color difference interface in `workspace/cpp/include/color_delta.hpp`
- [X] T052 [US2] Implement C++ color difference logic in `workspace/cpp/src/color_delta.cpp`
- [X] T053 [P] [US2] Implement C++ channel mapper interface in `workspace/cpp/include/channel_mapper.hpp`
- [X] T054 [US2] Implement C++ channel mapper logic in `workspace/cpp/src/channel_mapper.cpp`
- [X] T055 [US2] Implement C# channel mapping orchestrator in `workspace/csharp/Patterning.Core/Services/ChannelMappingService.cs`
- [X] T056 [US2] Implement C# channel mapping SQL repository in `workspace/csharp/Patterning.Infrastructure/Data/ChannelMappingRepository.cs`
- [X] T057 [US2] Implement C# dashboard channel editor view model in `workspace/csharp/PatterningOperatorDashboard/ViewModels/ChannelMappingViewModel.cs`
- [X] T058 [US2] Implement C# dashboard channel editor view in `workspace/csharp/PatterningOperatorDashboard/Views/ChannelMappingView.xaml`
- [X] T059 [US2] Implement C# mapped preview renderer in `workspace/csharp/PatterningOperatorDashboard/Rendering/MappedPreviewRenderer.cs`

**Checkpoint**: User Stories 1 and 2 work independently, with US2 starting from analyzed sample data if needed.

---

## Phase 5: User Story 3 - Convert the Design into a Production Model and Simulate It (Priority: P3)

**Goal**: A user can convert a mapped design to a 64, 128, or 256 grid, run an animated line-by-line simulation over TCP/IP, and observe diagnostics, channel activity, and lifecycle changes.

**Independent Test**: Start from a mapped sample design, convert to all supported grid sizes, start warning-only runs, confirm blocking errors prevent start, and validate start/pause/resume/reset/restart over TCP/IP.

### Tests for User Story 3

- [X] T060 [P] [US3] Add C++ grid conversion tests in `workspace/cpp/tests/grid_conversion_tests.cpp`
- [X] T061 [P] [US3] Add C++ command generation tests in `workspace/cpp/tests/command_generation_tests.cpp`
- [X] T062 [P] [US3] Add C control emulator protocol tests in `workspace/control-c/tests/control_protocol_tests.c`
- [X] T063 [P] [US3] Add C# simulation lifecycle tests in `workspace/csharp/Patterning.Tests/SimulationLifecycleTests.cs`
- [X] T064 [P] [US3] Add C# diagnostic gating tests in `workspace/csharp/Patterning.Tests/DiagnosticGatingTests.cs`
- [X] T065 [P] [US3] Add FPGA signal map testbench in `workspace/fpga/signal_map_tb.vhd`
- [X] T066 [P] [US3] Add PLC lifecycle simulation scenarios in `workspace/plc/scenarios/basic-run.json`

### Implementation for User Story 3

- [X] T067 [P] [US3] Implement C++ grid converter interface in `workspace/cpp/include/grid_converter.hpp`
- [X] T068 [US3] Implement C++ grid converter logic in `workspace/cpp/src/grid_converter.cpp`
- [X] T069 [P] [US3] Implement C++ command generator interface in `workspace/cpp/include/command_generator.hpp`
- [X] T070 [US3] Implement C++ pass-by-pass command generation in `workspace/cpp/src/command_generator.cpp`
- [X] T071 [US3] Extend C++ pattern processor for `grid.convert` in `workspace/cpp/src/pattern_processor_main.cpp`
- [X] T072 [P] [US3] Implement C control emulator state model in `workspace/control-c/include/control_state.h`
- [X] T073 [US3] Implement C control emulator lifecycle logic in `workspace/control-c/src/control_emulator.c`
- [X] T074 [US3] Implement C control emulator TCP server in `workspace/control-c/src/control_emulator_main.c`
- [X] T075 [US3] Implement C# production grid service in `workspace/csharp/Patterning.Core/Services/ProductionGridService.cs`
- [X] T076 [US3] Implement C# diagnostic service with blocking-error gate in `workspace/csharp/Patterning.Core/Services/DiagnosticService.cs`
- [X] T077 [US3] Implement C# simulation lifecycle orchestrator in `workspace/csharp/Patterning.Core/Services/SimulationLifecycleService.cs`
- [X] T078 [US3] Implement C# production grid SQL repository in `workspace/csharp/Patterning.Infrastructure/Data/ProductionGridRepository.cs`
- [X] T079 [US3] Implement C# simulation run SQL repository in `workspace/csharp/Patterning.Infrastructure/Data/SimulationRunRepository.cs`
- [X] T080 [US3] Implement C# dashboard simulation view model in `workspace/csharp/PatterningOperatorDashboard/ViewModels/SimulationDashboardViewModel.cs`
- [X] T081 [US3] Implement C# dashboard simulation view in `workspace/csharp/PatterningOperatorDashboard/Views/SimulationDashboardView.xaml`
- [X] T082 [US3] Implement FPGA signal map stub in `workspace/fpga/signal_map.vhd`
- [X] T083 [US3] Implement PLC Structured Text control stub in `workspace/plc/DigitalPatternControl.st`
- [X] T084 [US3] Implement C# PLC gateway simulation stub in `workspace/csharp/Patterning.Infrastructure/Gateways/PlcGatewayStub.cs`
- [X] T085 [US3] Implement C# FPGA timing gateway stub in `workspace/csharp/Patterning.Infrastructure/Gateways/FpgaTimingGatewayStub.cs`
- [X] T111 [US3] Implement C# gateway host command-line entry point in `workspace/csharp/Patterning.GatewayHost/Program.cs`

**Checkpoint**: User Stories 1, 2, and 3 work independently, and US3 can start from mapped sample data.

---

## Phase 6: User Story 4 - Export a Concept Report (Priority: P4)

**Goal**: A stakeholder can export printable HTML and structured JSON reports containing source summary, preview, metadata, palette, channel mapping, grid summary, diagnostics, and simulation results.

**Independent Test**: Complete analysis for a sample design, export both formats, and verify both contain all expected sections and no restricted or identifiable language.

### Tests for User Story 4

- [X] T086 [P] [US4] Add C# JSON report tests in `workspace/csharp/Patterning.Tests/JsonReportTests.cs`
- [X] T087 [P] [US4] Add C# HTML report tests in `workspace/csharp/Patterning.Tests/HtmlReportTests.cs`
- [X] T088 [P] [US4] Add C# confidentiality scan tests in `workspace/csharp/Patterning.Tests/ReportConfidentialityTests.cs`

### Implementation for User Story 4

- [X] T089 [P] [US4] Implement C# concept report model assembler in `workspace/csharp/Patterning.Core/Reports/ConceptReportBuilder.cs`
- [X] T090 [US4] Implement C# JSON report exporter in `workspace/csharp/Patterning.Core/Reports/JsonReportExporter.cs`
- [X] T091 [US4] Implement C# printable HTML report exporter in `workspace/csharp/Patterning.Core/Reports/HtmlReportExporter.cs`
- [X] T092 [US4] Implement C# report SQL query repository in `workspace/csharp/Patterning.Infrastructure/Data/ReportRepository.cs`
- [X] T093 [US4] Implement C# dashboard report export commands in `workspace/csharp/PatterningOperatorDashboard/ViewModels/ReportExportViewModel.cs`
- [X] T094 [US4] Implement C# dashboard report export view in `workspace/csharp/PatterningOperatorDashboard/Views/ReportExportView.xaml`

**Checkpoint**: All user stories are independently functional and report exports are validated.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Validate the full stack, improve documentation, and harden confidentiality and operations.

- [X] T095 [P] Update stack-specific developer documentation in `workspace/csharp/README.md`
- [X] T096 [P] Update C++ build and validation documentation in `workspace/cpp/README.md`
- [X] T097 [P] Update C control emulator documentation in `workspace/control-c/README.md`
- [X] T098 [P] Update SQL schema documentation in `workspace/sql/README.md`
- [X] T099 [P] Update PLC validation documentation in `workspace/plc/README.md`
- [X] T100 [P] Update FPGA validation documentation in `workspace/fpga/README.md`
- [ ] T101 Run full C# validation commands from `specs/001-digital-patterning-simulator/quickstart.md`
- [ ] T102 Run full C++ validation commands from `specs/001-digital-patterning-simulator/quickstart.md`
- [ ] T103 Run full C control emulator validation commands from `specs/001-digital-patterning-simulator/quickstart.md`
- [ ] T104 Run SQL schema validation commands from `specs/001-digital-patterning-simulator/quickstart.md`
- [ ] T105 Run PLC and FPGA stub validation commands from `specs/001-digital-patterning-simulator/quickstart.md`
- [X] T106 Run confidentiality term scan across `workspace/`, `docs/`, and `specs/001-digital-patterning-simulator/`
- [X] T107 [P] Add C# upload analysis timing validation in `workspace/csharp/Patterning.Tests/AnalysisPerformanceTests.cs`
- [X] T108 [P] Add C# simulation lifecycle latency validation in `workspace/csharp/Patterning.Tests/LifecycleLatencyTests.cs`
- [X] T109 [P] Add C# report export duration validation in `workspace/csharp/Patterning.Tests/ReportPerformanceTests.cs`
- [X] T113 [P] Add C# first-preview workflow timing validation in `workspace/csharp/Patterning.Tests/WorkshopPreviewWorkflowTimingTests.cs`
- [X] T114 [P] Add C# upload-to-simulation workflow timing validation in `workspace/csharp/Patterning.Tests/WorkshopSimulationWorkflowTimingTests.cs`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies; can start immediately.
- **Foundational (Phase 2)**: Depends on Setup completion; blocks all user stories.
- **User Story 1 (Phase 3)**: Depends on Foundational; recommended MVP.
- **User Story 2 (Phase 4)**: Depends on Foundational and can use fixture palette data, but integrates naturally after US1.
- **User Story 3 (Phase 5)**: Depends on Foundational and can use fixture mapped data, but integrates naturally after US2.
- **User Story 4 (Phase 6)**: Depends on Foundational and can use fixture concept/run data, but full report value comes after US1-US3.
- **Polish (Phase 7)**: Depends on the desired story set being complete.

### User Story Dependencies

- **US1 (P1)**: No dependency on other stories after Foundational.
- **US2 (P2)**: Independently testable with fixture palette data; production workflow uses US1 output.
- **US3 (P3)**: Independently testable with fixture mapped data; production workflow uses US1 and US2 output.
- **US4 (P4)**: Independently testable with fixture report data; full workflow uses US1, US2, and US3 output.

### Within Each User Story

- Tests should be written before implementation and fail before the corresponding implementation task.
- Domain models and protocol contracts come before services.
- Services come before dashboard views.
- Persistence repositories come before report/history views that depend on them.
- TCP/IP servers and clients must agree with `specs/001-digital-patterning-simulator/contracts/tcp-command-protocol.md` before lifecycle integration.

---

## Parallel Opportunities

- Setup tasks T002-T011 and T110 can run in parallel after T001 is created.
- Foundational model/protocol tasks T013-T018 and T024-T025 can run in parallel.
- Test tasks within each story can run in parallel because they target separate files.
- C#, C++, C, SQL, PLC, and FPGA implementation work can proceed in parallel after shared contracts are defined.
- Documentation tasks T095-T100 and performance validation tasks T107-T109 and T113-T114 can run in parallel during polish.

---

## Parallel Example: User Story 1

```bash
# Independent tests for upload/analyze/preview:
Task: "T027 [US1] Add C# upload validation tests in workspace/csharp/Patterning.Tests/UploadValidationTests.cs"
Task: "T029 [US1] Add C++ image metadata tests in workspace/cpp/tests/image_metadata_tests.cpp"
Task: "T030 [US1] Add C++ palette extraction tests in workspace/cpp/tests/palette_extraction_tests.cpp"
Task: "T031 [US1] Add SQL concept and palette repository tests in workspace/csharp/Patterning.Tests/ConceptRepositoryTests.cs"

# Independent implementation tracks:
Task: "T032 [US1] Implement C++ image input validator in workspace/cpp/include/image_validator.hpp"
Task: "T039 [US1] Implement C# upload validation service in workspace/csharp/Patterning.Core/Services/UploadValidationService.cs"
Task: "T042 [US1] Implement C# dashboard upload view model in workspace/csharp/PatterningOperatorDashboard/ViewModels/UploadDesignViewModel.cs"
```

## Parallel Example: User Story 3

```bash
# Cross-layer simulation work after foundational protocol is complete:
Task: "T067 [US3] Implement C++ grid converter interface in workspace/cpp/include/grid_converter.hpp"
Task: "T072 [US3] Implement C control emulator state model in workspace/control-c/include/control_state.h"
Task: "T075 [US3] Implement C# production grid service in workspace/csharp/Patterning.Core/Services/ProductionGridService.cs"
Task: "T082 [US3] Implement FPGA signal map stub in workspace/fpga/signal_map.vhd"
Task: "T083 [US3] Implement PLC Structured Text control stub in workspace/plc/DigitalPatternControl.st"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup.
2. Complete Phase 2 foundational models, SQL schema, protocol helpers, and TCP/IP abstractions.
3. Complete Phase 3 for upload, analysis, preview, metadata, and palette extraction.
4. Stop and validate US1 independently with C# xUnit tests and C++ CTest tests.
5. Demo sample design upload and preview before adding mapping or simulation.

### Incremental Delivery

1. Setup plus Foundational creates the cross-language skeleton and shared contracts.
2. US1 delivers upload, preview, metadata, and palette.
3. US2 adds editable 8-channel manufacturing mapping.
4. US3 adds grid conversion, diagnostics gating, TCP/IP simulation, PLC stubs, and FPGA stubs.
5. US4 adds HTML and JSON concept report export.
6. Polish validates the complete stack against `quickstart.md`.

### Parallel Team Strategy

1. One developer owns C# domain, dashboard, SQL repositories, and report export.
2. One developer owns C++ image processing, mapping, grid conversion, and command generation.
3. One developer owns C control emulator and TCP/IP lifecycle behavior.
4. One developer owns PLC Structured Text and FPGA VHDL stubs.
5. The team integrates through `contracts/tcp-command-protocol.md` and `contracts/sql-schema.sql`.

---

## Notes

- Every task uses the strict checklist format required by Spec Kit.
- `[P]` tasks target different files and can run without waiting on other incomplete tasks in the same phase.
- `[US1]`, `[US2]`, `[US3]`, and `[US4]` labels map directly to the user stories in `spec.md`.
- Keep all examples generic and confidentiality-safe.
- Keep implementation dependencies aligned with the selected stack: C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLCs.
