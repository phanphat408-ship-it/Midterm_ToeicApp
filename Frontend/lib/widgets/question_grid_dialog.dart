import 'package:flutter/material.dart';
import '../models/exam_models.dart';
import '../theme/app_theme.dart';

class QuestionGridDialog extends StatelessWidget {
  final List<QuestionItem> questions;
  final Map<int, String?> userAnswers;
  final int currentIndex;
  final Function(int) onSelectQuestion;

  const QuestionGridDialog({
    super.key,
    required this.questions,
    required this.userAnswers,
    required this.currentIndex,
    required this.onSelectQuestion,
  });

  @override
  Widget build(BuildContext context) {
    int answeredCount = userAnswers.values.where((ans) => ans != null && ans.isNotEmpty).length;

    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 24),
      decoration: const BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.vertical(top: Radius.circular(28)),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          // Drag handle
          Container(
            width: 48,
            height: 5,
            decoration: BoxDecoration(
              color: AppColors.border,
              borderRadius: BorderRadius.circular(10),
            ),
          ),
          const SizedBox(height: 16),
          // Header info
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text(
                    'Bảng câu hỏi',
                    style: TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                      color: AppColors.textDark,
                    ),
                  ),
                  Text(
                    'Đã làm: $answeredCount / ${questions.length} câu',
                    style: const TextStyle(fontSize: 13, color: AppColors.textMuted),
                  ),
                ],
              ),
              Row(
                children: [
                  _buildLegendItem('Đã làm', AppColors.emeraldGreen),
                  const SizedBox(width: 12),
                  _buildLegendItem('Chưa làm', AppColors.surfaceMuted),
                ],
              ),
            ],
          ),
          const SizedBox(height: 16),
          const Divider(height: 1),
          const SizedBox(height: 16),
          // Grid of questions
          ConstrainedBox(
            constraints: BoxConstraints(
              maxHeight: MediaQuery.of(context).size.height * 0.55,
            ),
            child: GridView.builder(
              shrinkWrap: true,
              itemCount: questions.length,
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 5,
                mainAxisSpacing: 10,
                crossAxisSpacing: 10,
                childAspectRatio: 1.1,
              ),
              itemBuilder: (context, index) {
                final q = questions[index];
                final selectedAns = userAnswers[q.id];
                final isAnswered = selectedAns != null && selectedAns.isNotEmpty;
                final isCurrent = index == currentIndex;

                return InkWell(
                  onTap: () {
                    Navigator.of(context).pop();
                    onSelectQuestion(index);
                  },
                  borderRadius: BorderRadius.circular(12),
                  child: Container(
                    decoration: BoxDecoration(
                      color: isAnswered ? AppColors.emeraldGreen : AppColors.surfaceMuted,
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(
                        color: isCurrent ? AppColors.primaryRoyalBlue : Colors.transparent,
                        width: isCurrent ? 2.5 : 1,
                      ),
                      boxShadow: isAnswered
                          ? [
                              BoxShadow(
                                color: AppColors.emeraldGreen.withValues(alpha: 0.3),
                                blurRadius: 4,
                                offset: const Offset(0, 2),
                              )
                            ]
                          : null,
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Text(
                          '${q.questionNumber}',
                          style: TextStyle(
                            fontSize: 13,
                            fontWeight: FontWeight.bold,
                            color: isAnswered ? Colors.white : AppColors.textDark,
                          ),
                        ),
                        if (isAnswered)
                          Text(
                            selectedAns,
                            style: const TextStyle(
                              fontSize: 11,
                              fontWeight: FontWeight.w800,
                              color: Colors.white,
                            ),
                          ),
                      ],
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildLegendItem(String label, Color color) {
    return Row(
      children: [
        Container(
          width: 12,
          height: 12,
          decoration: BoxDecoration(
            color: color,
            borderRadius: BorderRadius.circular(4),
            border: Border.all(color: AppColors.border),
          ),
        ),
        const SizedBox(width: 4),
        Text(
          label,
          style: const TextStyle(fontSize: 12, color: AppColors.textMuted),
        ),
      ],
    );
  }
}
