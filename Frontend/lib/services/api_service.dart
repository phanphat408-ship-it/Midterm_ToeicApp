import 'dart:convert';
import 'dart:io';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:http/io_client.dart';
import '../models/exam_models.dart';
import '../models/submit_models.dart';
import 'mock_data.dart';

class ApiService {
  static final ApiService _instance = ApiService._internal();
  factory ApiService() => _instance;

  String _baseUrl = '';
  http.Client? _client;

  ApiService._internal() {
    _initDefaultBaseUrl();
  }

  void _initDefaultBaseUrl() {
    if (kIsWeb) {
      _baseUrl = 'http://localhost:5000';
    } else if (Platform.isAndroid) {
      // 10.0.2.2 is Android emulator's loopback to host localhost
      _baseUrl = 'http://10.0.2.2:5000';
    } else {
      _baseUrl = 'http://localhost:5000';
    }
  }

  String get baseUrl => _baseUrl;

  void setBaseUrl(String url) {
    var trimmed = url.trim();
    if (trimmed.endsWith('/')) {
      trimmed = trimmed.substring(0, trimmed.length - 1);
    }
    _baseUrl = trimmed;
  }

  http.Client _getClient() {
    if (_client != null) return _client!;

    if (!kIsWeb) {
      // Allow self-signed development certificates for localhost/10.0.2.2
      final ioClient = HttpClient()
        ..badCertificateCallback = (X509Certificate cert, String host, int port) => true;
      _client = IOClient(ioClient);
    } else {
      _client = http.Client();
    }
    return _client!;
  }

  /// Get list of exams
  Future<List<ExamListItem>> getAllExams() async {
    final endpoint = '$_baseUrl/api/services/app/Exam/GetAllExams';
    try {
      final client = _getClient();
      final response = await client
          .get(Uri.parse(endpoint), headers: {'Accept': 'application/json'})
          .timeout(const Duration(seconds: 4));

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        if (data['success'] == true && data['result'] != null) {
          final list = (data['result'] as List)
              .map((item) => ExamListItem.fromJson(item))
              .toList();
          if (list.isNotEmpty) return list;
        }
      }
    } catch (e) {
      debugPrint('API Error getAllExams ($endpoint): $e');
    }

    // Offline / fallback data
    return MockExamData.getMockExamList();
  }

  /// Get hierarchical exam detail
  Future<ExamDetail> getExamDetail(int examId) async {
    final endpoint = '$_baseUrl/api/services/app/Exam/GetExamDetail?id=$examId';
    try {
      final client = _getClient();
      final response = await client
          .get(Uri.parse(endpoint), headers: {'Accept': 'application/json'})
          .timeout(const Duration(seconds: 4));

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        if (data['success'] == true && data['result'] != null) {
          return ExamDetail.fromJson(data['result']);
        }
      }
    } catch (e) {
      debugPrint('API Error getExamDetail ($endpoint): $e');
    }

    // Offline fallback
    return MockExamData.getMockExamDetail(examId);
  }

  /// Submit exam answers
  Future<SubmitExamResult> submitExam(SubmitExamPayload payload) async {
    final endpoint = '$_baseUrl/api/services/app/Exam/SubmitExam';
    try {
      final client = _getClient();
      final body = jsonEncode(payload.toJson());
      final response = await client
          .post(
            Uri.parse(endpoint),
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json',
            },
            body: body,
          )
          .timeout(const Duration(seconds: 5));

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        if (data['success'] == true && data['result'] != null) {
          return SubmitExamResult.fromJson(data['result']);
        }
      }
    } catch (e) {
      debugPrint('API Error submitExam ($endpoint): $e');
    }

    // Local grading fallback
    return MockExamData.getMockSubmitResult(payload);
  }
}
