function drawRadialChart({ proteins, fats, carbs }) {
    const canvas = document.getElementById('radialChartCanvas');
    const ctx = canvas.getContext('2d');

    const total = proteins + fats + carbs;
    if (total === 0) return;

    const values = [proteins, fats, carbs];
    const colors = ['#00bfff', '#ffc107', '#ff5252']; // proteins, fats, carbs

    const maxValue = Math.max(...values);
    const baseAngle = (2 * Math.PI) / 3;
    const centerX = canvas.width / 2;
    const centerY = canvas.height / 2;
    const maxRadius = Math.min(canvas.width, canvas.height) / 2 - 10;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // Draw each sector
    for (let i = 0; i < 3; i++) {
        const startAngle = i * baseAngle - Math.PI / 2;
        const endAngle = startAngle + baseAngle;
        const radius = maxRadius * (values[i] / maxValue);

        ctx.beginPath();
        ctx.moveTo(centerX, centerY);
        ctx.arc(centerX, centerY, radius, startAngle, endAngle);
        ctx.closePath();

        ctx.fillStyle = colors[i];
        ctx.fill();
    }

    // Optional: white circle in center for visual separation
    ctx.beginPath();
    ctx.arc(centerX, centerY, 30, 0, 2 * Math.PI);
    ctx.fillStyle = '#fff';
    ctx.fill();
}

// Ensure canvas has fixed size (e.g., 250x250)
document.addEventListener('DOMContentLoaded', () => {
    const canvas = document.getElementById('radialChartCanvas');
    if (canvas) {
        canvas.width = 250;
        canvas.height = 250;
    }
});