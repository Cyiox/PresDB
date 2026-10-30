# PBV Matching Process Documentation

## Overview

This script matches properties from the CEDAC Forecast database with Project-Based Voucher (PBV) contracts from HUD. The matching process uses multiple strategies with confidence scoring and flagging to identify the most likely matches while highlighting potential issues for manual review.

---

## Input Files

1. **Forecast File**: `Forecast Data/Current/2030 Forecast Master 2-5-26.xlsx`
   - Contains property information from CEDAC database
   - Key columns: `PropertyName`, `City`, `Street`, `TotalUnits`, `Project/AKA`

2. **PBV File**: `PBV Matching/PBVExtractForCEDAC.xlsx`
   - Contains HUD PBV contract data
   - Key columns: `Project`, `City/Town`, `Address`, `Total Units`, `Contract Number`, `RAA #`

---

## Output Files

1. **PBV_Matching3.xlsx**
   - Complete merged data with ALL columns from both input files
   - Includes match metadata (confidence, method, flags)
   - Use for detailed analysis

2. **2030_Forecast_PBV_Match_Summary.xlsx**
   - Simplified view with key columns only
   - **Sorted by confidence (lowest first)** - questionable matches appear at top
   - Use for quick review and decision-making

3. **Unmatched_PBV_Projects.xlsx**
   - PBV contracts that didn't match any Forecast property
   - Review to identify potential additions to Forecast database

---

## Matching Strategies

The script uses 4 sequential matching strategies. Once a property is matched, it's excluded from subsequent strategies.

### Strategy 1: Exact Name Match
**Criteria:**
- `PropertyName` (Forecast) must **exactly** match `Project` (PBV)
- Both properties must be in the **same city**

**Confidence Scoring:**
- Starts at **100%**
- Adjusted based on unit count compatibility (see Unit Count Scoring below)
- Final range: **50-100%**

---

### Strategy 2: Fuzzy Name Match
**Criteria:**
- Property names must be **≥85% similar** after normalization
- Both properties must be in the **same city**

**Name Normalization Process:**
1. Convert to lowercase
2. Standardize abbreviations:
   - `street` → `st`
   - `avenue` → `ave`
   - `building` → `bldg`
   - `veterans` → `veteran`
3. Remove common words: `apartments`, `apts`, `housing`, `residence`, `senior`, `estates`, `homes`, `village`, `towers`, `place`, `commons`, `court`, `manor`, `the`
4. Remove punctuation
5. Remove extra spaces

**Example:**
- "The Veterans Housing Apartments" → "veteran"
- "Veteran's House" → "veteran"
- **Similarity: ~90%** ✓

**Confidence Scoring:**
- Starts with base fuzzy match score (**85-100%**)
- Adjusted for:
  - Unit count compatibility: **-15 to +5**
  - Phase mismatch: **-20**
  - Expansion mismatch: **-10**
- Final range: **50-99%**

---

### Strategy 3: Project/AKA Match
**Criteria:**
- Matches using the `Project/AKA` field from Forecast (alternate names)
- AKA field is split by separators: `,`, `/`, `|`, `;`
- Each AKA name is compared to PBV project names
- Must be **≥85% similar** after normalization
- Both properties must be in the **same city**

**Confidence Scoring:**
- Same as Fuzzy Name Match
- Final range: **50-99%**

---

### Strategy 4: Street Address Match
**Criteria (ALL must be met):**
- Both properties must be in the **same city**
- Street numbers must be **within 20** of each other
- Street **names** must be **≥90% similar** (without numbers/suffixes)
- Full normalized addresses must be **≥80% similar**

**Address Normalization Process:**
1. Convert to lowercase
2. Standardize suffixes:
   - `street`/`strt` → `st`
   - `road` → `rd`
   - `avenue` → `ave`
   - `drive` → `dr`
   - `lane` → `ln`
   - `court` → `ct`
3. Remove punctuation
4. Remove extra spaces

**Street Name Extraction:**
- Removes street numbers (e.g., "123")
- Removes suffixes (e.g., "St", "Ave")
- Prevents "Columbus Ave" from matching "Columbus St"

**Example Matches:**
- ✓ "123 Main St" matches "125 Main Street" (numbers within 20, same name)
- ✗ "123 Main St" does NOT match "123 Main Ave" (different suffix)
- ✗ "123 Main St" does NOT match "543 Main St" (numbers differ by 420)

**Confidence Scoring:**
- Starts with base address similarity (**80-100%**)
- Adjusted for:
  - Unit count compatibility: **-15 to +5**
  - Even/odd parity mismatch: **-15**
  - Phase mismatch: **-20**
  - Expansion mismatch: **-10**
- Final range: **40-95%**

---

## Unit Count Scoring

Unit count comparisons significantly impact confidence scores to identify potential mismatches.

| Unit Difference | Adjustment | Flag | Description |
|----------------|------------|------|-------------|
| Exact match (0) | **+5** | None | Strong indicator of same property |
| Within 2 units | **+3** | None | Very likely same property |
| 3-10 units | **-5** | `UNIT_MISMATCH_MODERATE` | May indicate data discrepancy |
| 11-30 units | **-10** | `UNIT_MISMATCH_LARGE` | Significant concern |
| >30 units | **-15** | `UNIT_MISMATCH_VERY_LARGE` | Likely different properties |
| Missing data | **0** | `MISSING_UNITS` | Cannot assess |

**Why This Matters:**
- Exact unit match strengthens confidence even with minor name variations
- Large unit differences can reveal:
  - Phase I vs Phase II confusion
  - Expansion projects vs original projects
  - Data entry errors
  - Completely different properties

---

## Flags Reference

Flags are added to highlight potential issues for manual review. Multiple flags can apply to a single match.

### Unit Count Flags

| Flag | Meaning | Penalty |
|------|---------|---------|
| `MISSING_UNITS` | Unit count data missing from one or both sources | 0 |
| `UNIT_MISMATCH_MODERATE` | Units differ by 3-10 | -5 |
| `UNIT_MISMATCH_LARGE` | Units differ by 11-30 | -10 |
| `UNIT_MISMATCH_VERY_LARGE` | Units differ by >30 | -15 |

### Phase/Project Type Flags

| Flag | Meaning | Penalty |
|------|---------|---------|
| `PHASE_MISMATCH` | One is Phase I, the other is Phase II (or similar) | -20 |
| `EXPANSION_MISMATCH` | One project has "expansion"/"addition" in name, other doesn't | -10 |

**Example:**
- "Malden Mills Phase I" vs "Malden Mills Phase II" → `PHASE_MISMATCH`
  - High name similarity (~98%) but **clearly different properties**
  - Confidence reduced from 98% to ~78%

### Address Flags

| Flag | Meaning | Penalty |
|------|---------|---------|
| `ODD_EVEN_MISMATCH` | Street numbers have different parity (e.g., 559 vs 560) | -15 |

**Why This Matters:**
- In most cities, odd numbers are on one side of the street, even on the other
- Different parity suggests different buildings
- **Example**: 559 Main St vs 560 Main St are likely across the street from each other

---

## Confidence Score Interpretation

| Confidence Range | Interpretation | Action |
|-----------------|----------------|--------|
| **95-100%** | Very high confidence | Likely correct match; quick review |
| **85-94%** | High confidence | Review for any flags |
| **70-84%** | Moderate confidence | Manual review recommended |
| **50-69%** | Low confidence | Careful review required; likely issues |
| **<50%** | Very low confidence | Probably incorrect match |

**Note:** Matches are sorted by confidence (lowest first) in the Summary file, so problematic matches appear at the top for immediate attention.

---


## Technical Notes

### City Validation
- All matches **require** cities to match exactly (case-insensitive)
- Prevents matching properties in different municipalities
- "Boston" ≠ "South Boston"

### Sequential Matching
- Strategies run in order: Exact → Fuzzy → AKA → Street
- Once matched, property is excluded from later strategies
- Prevents duplicate matches

### Street Number Range
- Street numbers must be within **20** of each other
- Example: 100-120 Main St ✓, 100-125 Main St ✗
- Prevents matching properties far apart on same street

### Performance
- Fuzzy matching uses Levenshtein distance algorithm
- ~200 matches typically found from ~500 PBV contracts
- Runtime: 10-30 seconds depending on data size

---

## Change Log

### Version 2.0 (Current)
- Added even/odd parity check for street numbers
- Fixed "Total Units" column name recognition
- Enhanced street name matching to prevent suffix mismatches
- Added expansion project detection
- Improved unit count comparison with detailed flags
- Added Base_Confidence vs Match_Confidence tracking

### Version 1.0
- Initial implementation
- Basic fuzzy matching
- City validation
- Unit count comparison

---

## Questions or Issues?

If you encounter unexpected matches or mismatches:
1. Check the confidence score and flags
2. Review the Base_Confidence to see original similarity
3. Verify unit counts match expectations
4. Check for phase/expansion keywords in names
5. Confirm addresses are truly for same building

For persistent issues, consider:
- Adjusting similarity thresholds (currently 85% for names, 80% for addresses)
- Modifying penalty values
- Adding new flags for specific scenarios
