# Acceptance Criteria - MyStore E-Commerce App

## Overview
This document defines the acceptance criteria for the MyStore e-commerce application. Each criterion must be tested and verified before the application is considered complete.

---

## ✅ Functional Requirements

### FC-1: Display Product List
**Requirement**: App fetches and displays products from API
- **Acceptance Criteria**:
  - [ ] App loads products within 10 seconds of opening Products page
  - [ ] At least 5 sample products display correctly
  - [ ] Product list shows: Name, Image, Price, Stock quantity
  - [ ] Product grid is responsive (2 columns on mobile, adapts to screen size)
  - [ ] Products display with proper formatting and readability
  - [ ] Error message shows if API connection fails
  - [ ] Refresh button works and reloads products

**Test Steps**:
1. Open app → Navigate to Products tab
2. Verify products load and display correctly
3. Press Refresh button
4. Close network → Refresh → Verify error message
5. Result: PASS/FAIL

---

### FC-2: Add Product to Cart
**Requirement**: User can add products to shopping cart
- **Acceptance Criteria**:
  - [ ] Tap product → Navigate to product detail page
  - [ ] Product detail shows: full name, description, price, stock, image
  - [ ] User can increase/decrease quantity (+ / - buttons)
  - [ ] Quantity cannot exceed available stock
  - [ ] "Add to Cart" button adds item with selected quantity
  - [ ] Success message displays after adding
  - [ ] Cart counter badge updates in top right
  - [ ] Multiple different products can be added

**Test Steps**:
1. Browse Products → Select a product
2. Click + button multiple times to increase quantity
3. Verify quantity doesn't exceed stock
4. Click "Add to Cart"
5. Verify success message
6. Check cart badge count updated
7. Result: PASS/FAIL

---

### FC-3: Cart Persistence
**Requirement**: Cart items persist after app restart
- **Acceptance Criteria**:
  - [ ] Add at least 2 different products to cart
  - [ ] Verify items appear in Cart page
  - [ ] Close app completely (kill process)
  - [ ] Reopen app
  - [ ] Cart items still present with correct quantities
  - [ ] Item count matches original
  - [ ] Total price calculated correctly
  - [ ] Data stored in local SQLite database

**Test Steps**:
1. Add Product 1 (Qty: 2) + Product 2 (Qty: 3) to cart
2. Note total: should be 5 items
3. Close app (swipe to close on mobile, Alt+F4 on desktop)
4. Wait 5 seconds
5. Reopen app → Navigate to Cart
6. Verify 5 items still in cart
7. Result: PASS/FAIL

---

### FC-4: Manage Cart Items
**Requirement**: User can modify and remove cart items
- **Acceptance Criteria**:
  - [ ] Each cart item displays: name, unit price, quantity, subtotal
  - [ ] User can update quantity (or use dedicated buttons)
  - [ ] Changing quantity updates subtotal immediately
  - [ ] "Remove" button removes item from cart
  - [ ] Remove action shows confirmation dialog
  - [ ] Total price updates correctly after changes
  - [ ] "Clear Cart" button removes all items
  - [ ] Clear Cart shows confirmation dialog

**Test Steps**:
1. Add 2 items to cart
2. Change first item quantity from 2 to 5
3. Verify subtotal updated: Unit Price × 5
4. Verify total price updated
5. Click "Remove" on second item → Confirm
6. Verify item removed and total recalculated
7. Click "Clear Cart" → Confirm
8. Verify all items removed
9. Result: PASS/FAIL

---

### FC-5: Checkout with Validation
**Requirement**: User fills checkout form with validation
- **Acceptance Criteria**:
  - [ ] Checkout page shows order summary (items + total)
  - [ ] Form has fields: Name, Address, Phone Number, Notes (optional)
  - [ ] Name field required and max 256 characters
  - [ ] Address field required and max 512 characters
  - [ ] Phone field required, validates minimum 10 digits
  - [ ] Notes field optional (max 1000 characters)
  - [ ] Submit button disabled until all required fields valid
  - [ ] Error messages show for each invalid field
  - [ ] Submit button says "Place Order"

**Test Steps**:
1. Go to Cart → Click "Checkout"
2. Leave all fields empty → Verify button disabled
3. Enter invalid phone (5 digits) → Verify error message
4. Fill all required fields correctly
5. Verify submit button enabled
6. (Don't submit yet)
7. Result: PASS/FAIL

---

### FC-6: Order Submission & Cart Clear
**Requirement**: Order submitted to API and cart cleared on success
- **Acceptance Criteria**:
  - [ ] Filled checkout form
  - [ ] Click "Place Order" button
  - [ ] Button shows "Submitting..." during request
  - [ ] API receives POST request with all order data
  - [ ] Success response received (< 10 seconds)
  - [ ] Success dialog shows Order ID
  - [ ] User returned to Products page automatically
  - [ ] Cart is now empty
  - [ ] Cart badge shows 0
  - [ ] Cart page displays "Your cart is empty"

**Test Steps**:
1. Add products to cart
2. Go to Checkout → Fill form
3. Click "Place Order"
4. Wait for response
5. Verify success message with Order ID
6. Navigate to Cart page
7. Verify cart empty with message
8. Verify cart badge shows 0
9. Result: PASS/FAIL

---

### FC-7: Network Error Handling
**Requirement**: App handles network errors gracefully
- **Acceptance Criteria**:
  - [ ] Disconnect network (or use offline mode)
  - [ ] Try to load products → Error message displays
  - [ ] Try to submit order → Error message displays
  - [ ] Error includes helpful message (not technical details)
  - [ ] Cart data not lost during failed submission
  - [ ] Reconnect network → Retry succeeds
  - [ ] App doesn't crash on network errors

**Test Steps**:
1. Airplane mode ON (mobile) or disable WiFi (desktop)
2. Open Products tab → Verify error message
3. Add item to cart (offline) → Should work
4. Try to submit order → Verify error message
5. Verify cart still has items
6. Airplane mode OFF → Retry submission
7. Result: PASS/FAIL

---

## 🧪 Technical Requirements

### TR-1: Unit Tests Pass
**Requirement**: All CartService unit tests pass
- **Acceptance Criteria**:
  - [ ] Minimum 12 unit tests for CartService
  - [ ] Tests cover: Add, Update, Remove, Clear, Calculate totals
  - [ ] All tests pass without errors
  - [ ] Tests use in-memory SQLite database
  - [ ] Test code is readable with clear assertions

**Verification**:
```bash
dotnet test MyStore.Tests\MyStore.Tests.csproj
# Expected: 12 passed, 0 failed
```

**Result**: PASS/FAIL ✅ / ❌

---

### TR-2: Code Quality
**Requirement**: Code follows best practices
- **Acceptance Criteria**:
  - [ ] No compilation warnings
  - [ ] All classes have XML documentation comments
  - [ ] MVVM pattern consistently applied
  - [ ] Dependency injection used throughout
  - [ ] No hardcoded values (except constants)
  - [ ] Error handling in all async operations
  - [ ] Thread-safe operations (locks for cart)

**Verification**:
```bash
dotnet build MyStore.sln /WarnAsError
# Expected: No warnings or errors
```

**Result**: PASS/FAIL ✅ / ❌

---

### TR-3: Database Integrity
**Requirement**: SQLite database works correctly
- **Acceptance Criteria**:
  - [ ] Database auto-creates on first run
  - [ ] Tables created with correct schema
  - [ ] Sample data seeded automatically
  - [ ] Data persists after app restart
  - [ ] No database corruption issues
  - [ ] Foreign key constraints (if applicable) enforced

**Verification**:
```bash
# Check database file exists
# Windows: %LOCALAPPDATA%/MyStore/mystore.db
# Android: /data/data/com.mystore/files/mystore.db
# iOS: ~/Library/Application Support/mystore.db
```

**Result**: PASS/FAIL ✅ / ❌

---

### TR-4: Cross-Platform Compatibility
**Requirement**: App builds and runs on multiple platforms
- **Acceptance Criteria**:
  - [ ] Builds successfully on Windows (MAUI)
  - [ ] Runs on Android emulator
  - [ ] Builds successfully for iOS (if on macOS)
  - [ ] WASM version runs in browser
  - [ ] UI responsive on all screen sizes
  - [ ] No platform-specific crashes

**Build Commands**:
```bash
# Windows
dotnet build MyStore.sln -c Debug

# Android
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -f net8.0-android

# WebAssembly
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -f net8.0-wasm

# iOS (macOS only)
dotnet build MyStore.Mobile\MyStore.Mobile.csproj -f net8.0-ios
```

**Result**: PASS/FAIL ✅ / ❌

---

## 📦 Deliverables

### DL-1: Source Code
**Requirement**: Complete, organized source code
- **Acceptance Criteria**:
  - [ ] MyStore.Core project contains all models, services, database
  - [ ] MyStore.Mobile project contains all views and viewmodels
  - [ ] MyStore.Tests project contains unit tests
  - [ ] All files follow naming conventions
  - [ ] Solution file (MyStore.sln) opens without errors
  - [ ] No binary files or build artifacts checked in

**Verification**:
```bash
git status
# Expected: Clean working directory, no uncommitted changes
```

**Result**: PASS/FAIL ✅ / ❌

---

### DL-2: Documentation
**Requirement**: Complete documentation provided
- **Acceptance Criteria**:
  - [ ] README.md with build/run instructions (this file)
  - [ ] report.md with project details and design documentation
  - [ ] acceptance_criteria.md (this checklist)
  - [ ] Code comments on all complex logic
  - [ ] API documentation (endpoint descriptions)
  - [ ] Troubleshooting guide included

**Verification**:
```bash
# Check files exist
test -f README.md && test -f report.md && test -f acceptance_criteria.md
# Expected: All three files present and readable
```

**Result**: PASS/FAIL ✅ / ❌

---

### DL-3: Release Builds
**Requirement**: App can be packaged for release
- **Acceptance Criteria**:
  - [ ] Android .aab file can be generated
  - [ ] iOS .ipa file can be generated (on macOS)
  - [ ] WASM wwwroot folder for deployment
  - [ ] Windows MSIX package can be built
  - [ ] No personal data in release builds
  - [ ] Version number incremented

**Verification**:
```bash
# Android
dotnet publish -c Release -f net8.0-android -o publish/android

# WASM
dotnet publish -c Release -f net8.0-wasm -o publish/wasm
```

**Result**: PASS/FAIL ✅ / ❌

---

## 🎯 Test Results Summary

### Overall Status: _______________ (PASS / FAIL)

| Category | Tests | Passed | Failed | Notes |
|----------|-------|--------|--------|-------|
| Functional | 7 | ___ | ___ | Product display, cart, checkout |
| Technical | 4 | ___ | ___ | Tests, code quality, DB, cross-platform |
| Deliverables | 3 | ___ | ___ | Source, docs, release |
| **TOTAL** | **14** | **___** | **___** | **Pass = 14/14** |

---

## 📋 Sign-Off

**Tester Name**: ___________________________

**Date**: ___________________________

**Signature**: ___________________________

**Notes/Issues Found**:
```
(List any issues or bugs found during testing)
```

---

## ✅ Final Checklist

- [ ] All 14 acceptance criteria tested
- [ ] 12/12 unit tests passing
- [ ] No build warnings or errors
- [ ] All deliverables present
- [ ] Documentation complete
- [ ] Release builds successful
- [ ] Cross-platform verified
- [ ] Ready for submission

**Status**: ⭐ READY FOR GRADING
