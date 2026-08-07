# CCOG Unified Skill Schema

## Overview

This document describes the **unified CCOG Skill model** introduced in ticket **AP-57**. The goal is to consolidate all skill-related data from multiple sources into a single, canonical entity (`CCOG_Skill`) that serves as the system of record for employee skills across the organisation.

---

## Table: `CCOG_Skills`

| Column              | Type            | Nullable | Default        | Description                                                        |
|---------------------|-----------------|----------|----------------|--------------------------------------------------------------------|
| `Id`                | `int`           | No       | Identity       | Primary key, auto-incremented.                                     |
| `SkillName`         | `nvarchar(200)` | No       | —              | Name of the skill (e.g. "C#", "AWS Lambda").                       |
| `Category`          | `nvarchar(100)` | No       | —              | Category grouping (e.g. "Programming Language", "Cloud").          |
| `Level`             | `nvarchar(50)`  | No       | —              | Proficiency level: Beginner, Intermediate, Advanced, Expert.       |
| `LastCertifiedDate` | `datetime2`     | Yes      | `NULL`         | Date the skill was last certified/validated.                       |
| `CertificationName` | `nvarchar(300)` | Yes      | `NULL`         | Name of the certification (e.g. "AWS Solutions Architect").        |
| `Owner`             | `nvarchar(200)` | No       | —              | The employee or entity this skill belongs to.                      |
| `Source`            | `nvarchar(100)` | No       | —              | Origin of the record: `DOS`, `SelfReport`, `ManagerValidation`.    |
| `IsActive`          | `bit`           | No       | `1` (true)     | Soft-delete flag; `false` = logically deleted.                     |
| `CreatedAt`         | `datetime2`     | No       | `GETUTCDATE()` | UTC timestamp when the record was created.                         |
| `UpdatedAt`         | `datetime2`     | No       | `GETUTCDATE()` | UTC timestamp when the record was last updated.                    |

### Indexes

| Index                      | Column(s)  | Purpose                                     |
|---------------------------|------------|----------------------------------------------|
| `IX_CCOG_Skills_Owner`    | `Owner`    | Fast lookups by employee/owner.              |
| `IX_CCOG_Skills_Category` | `Category` | Fast filtered queries by skill category.     |
| `IX_CCOG_Skills_IsActive` | `IsActive` | Efficient soft-delete filtering.             |

---

## Ownership Model

The unified skill model supports **three distinct sources** of skill data, each with a clear ownership responsibility:

### 1. DOS (Department of Skills) — Taxonomy Maintenance

- **Responsibility**: DOS maintains the **master taxonomy** of skill names and categories.
- **What they control**: The canonical list of `SkillName` and `Category` values that are available for selection.
- **Process**: DOS periodically reviews and updates the taxonomy to reflect industry changes, new technologies, and organisational needs.
- **Source value**: `DOS`

### 2. Employee Self-Report

- **Responsibility**: Employees **self-report** their own skills and proficiency levels.
- **What they control**: Individual skill records tied to their `Owner` identifier, including `Level`, `CertificationName`, and `LastCertifiedDate`.
- **Process**: Employees add or update skills through the API or a future front-end interface.
- **Source value**: `SelfReport`

### 3. Manager Validation

- **Responsibility**: Managers **validate and endorse** employee skill claims.
- **What they control**: Managers can confirm, adjust, or flag skills reported by their direct reports.
- **Process**: A manager reviews pending skill self-reports and either approves them or adjusts the proficiency level.
- **Source value**: `ManagerValidation`

---

## Maintenance Responsibilities

| Responsibility             | Owner              | Frequency         |
|----------------------------|--------------------|--------------------|
| Taxonomy updates           | DOS                | Quarterly          |
| Skill self-reporting       | Individual employees | On-demand        |
| Manager validation         | Line managers      | Monthly / On-demand |
| Data quality audits        | DOS + Engineering  | Semi-annually      |
| Schema/model changes       | Engineering        | As needed (PR-based) |

---

## Update Cadence

| Activity                       | Cadence          | Notes                                                 |
|--------------------------------|------------------|-------------------------------------------------------|
| DOS taxonomy refresh           | Quarterly        | New skills added, deprecated skills soft-deleted.     |
| Employee self-report window    | Always open      | Employees can update at any time.                     |
| Manager validation cycle       | Monthly          | Managers review new/updated skills from their team.   |
| Certification expiry checks    | Monthly (automated) | Flag skills where `LastCertifiedDate` > threshold. |

---

## Migration Strategy

### Phase 1 — Schema Deployment
1. Create the `CCOG_Skills` table using EF Core migrations (`dotnet ef migrations add InitialCCOGSkills`).
2. Apply migrations to dev/staging environments.
3. Validate the schema against this document.

### Phase 2 — Data Import
1. Export skill data from existing legacy sources (spreadsheets, HR systems, ad-hoc databases).
2. Map legacy fields to the unified schema columns.
3. Use the **`POST /api/ccog_skills/bulk`** endpoint to bulk-import records.
4. Assign the correct `Source` value for each imported record.

### Phase 3 — Validation & Reconciliation
1. Run data-quality checks: missing fields, duplicate skills per owner, invalid categories.
2. Generate reconciliation reports for DOS review.
3. Soft-delete (`IsActive = false`) any records that fail quality checks until resolved.

### Phase 4 — Cut-over
1. Update downstream consumers to read from the new `CCOG_Skills` table.
2. Deprecate legacy data sources.
3. Monitor API usage and error rates for the first 30 days.

---

## API Endpoints

| Method   | Route                     | Description                                      |
|----------|---------------------------|--------------------------------------------------|
| `GET`    | `/api/ccog_skills`        | List all active skills (supports `?category=`, `?owner=`, `?source=` filters). |
| `GET`    | `/api/ccog_skills/{id}`   | Get a single active skill by ID.                 |
| `POST`   | `/api/ccog_skills`        | Create a new skill record.                       |
| `PUT`    | `/api/ccog_skills/{id}`   | Update an existing skill record.                 |
| `DELETE` | `/api/ccog_skills/{id}`   | Soft-delete a skill record (`IsActive = false`). |
| `POST`   | `/api/ccog_skills/bulk`   | Bulk import multiple skill records.              |

---

## Running Migrations

```bash
# From the TestingApi project directory
dotnet ef migrations add InitialCCOGSkills --project TestingApi
dotnet ef database update --project TestingApi
```

> **Note**: Ensure the connection string in `appsettings.json` (`ConnectionStrings:DefaultConnection`) points to your target SQL Server instance before running migrations.

---

## Future Considerations

- **Role-based access control (RBAC)**: Restrict write operations by source (e.g. only DOS can modify taxonomy records, employees can only edit their own self-reports).
- **Audit log**: Track every change to a skill record with before/after snapshots.
- **Skill versioning**: Maintain a history table for proficiency-level changes over time.
- **Integration events**: Publish domain events when skills are created, updated, or deleted to support downstream analytics.
