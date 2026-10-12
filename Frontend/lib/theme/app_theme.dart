import 'package:flutter/material.dart';

class AppColors {
  // Primary Palette
  static const Color primaryRoyalBlue = Color(0xFF1E3A8A); // #1E3A8A
  static const Color primaryDark = Color(0xFF0F172A);
  static const Color primaryLight = Color(0xFF3B82F6);
  static const Color royalBlueSubtle = Color(0xFFEFF6FF);

  // Success / Emerald Green
  static const Color emeraldGreen = Color(0xFF10B981); // #10B981
  static const Color emeraldDark = Color(0xFF059669);
  static const Color emeraldLight = Color(0xFFD1FAE5);

  // Error / Warning
  static const Color redError = Color(0xFFEF4444); // #EF4444
  static const Color redLight = Color(0xFFFEE2E2);
  static const Color amberWarning = Color(0xFFF59E0B);
  static const Color amberLight = Color(0xFFFEF3C7);

  // Backgrounds & Neutrals
  static const Color background = Color(0xFFF8FAFC); // #F8FAFC
  static const Color cardBg = Colors.white;
  static const Color surfaceMuted = Color(0xFFF1F5F9);
  static const Color border = Color(0xFFE2E8F0);

  // Typography
  static const Color textDark = Color(0xFF0F172A);
  static const Color textMuted = Color(0xFF64748B);
  static const Color textLight = Color(0xFF94A3B8);
}

class AppTheme {
  static ThemeData get lightTheme {
    return ThemeData(
      useMaterial3: true,
      colorScheme: ColorScheme.fromSeed(
        seedColor: AppColors.primaryRoyalBlue,
        primary: AppColors.primaryRoyalBlue,
        secondary: AppColors.emeraldGreen,
        error: AppColors.redError,
        surface: AppColors.background,
      ),
      scaffoldBackgroundColor: AppColors.background,
      appBarTheme: const AppBarTheme(
        backgroundColor: AppColors.primaryRoyalBlue,
        foregroundColor: Colors.white,
        elevation: 0,
        centerTitle: false,
        titleTextStyle: TextStyle(
          fontSize: 20,
          fontWeight: FontWeight.bold,
          color: Colors.white,
          letterSpacing: -0.3,
        ),
      ),
      cardTheme: CardThemeData(
        color: AppColors.cardBg,
        elevation: 0,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(16),
          side: const BorderSide(color: AppColors.border, width: 1),
        ),
      ),
      elevatedButtonTheme: ElevatedButtonThemeData(
        style: ElevatedButton.styleFrom(
          backgroundColor: AppColors.primaryRoyalBlue,
          foregroundColor: Colors.white,
          elevation: 0,
          padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 14),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
          ),
          textStyle: const TextStyle(
            fontSize: 15,
            fontWeight: FontWeight.w600,
            letterSpacing: -0.2,
          ),
        ),
      ),
    );
  }
}
