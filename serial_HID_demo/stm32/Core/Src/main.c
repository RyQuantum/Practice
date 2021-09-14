/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "dma.h"
#include "usart.h"
#include "gpio.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "stdio.h"
#include "string.h"
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */
uint8_t str1[] = "==== Ryan welcome you! ====\r\n";
uint8_t str2[] = "MIIBqTCCARICAQAwaTELMAkGA1UEBhMCQ04xETAPBgNVBAgMCExpYW9OaW5nMQ8w\nDQYDVQQHDAZEYUxpYW4xDzANBgNVBAoMBmRldm9wczEQMA4GA1UECwwHdW5pY29y\nbjETMBEGA1UEAwwKZGV2b3BzLmNvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkC\ngYEAxHsuCR2C5vw9AoxkWLfVcSnTj4xaNE1GiXdrDRSPxN4t3BJRy5YIC/f6mJGl\nl04UaHNk8Hu+hz5H/lpxWPua+cY+uzTEwbeDh9M6KJtoFeH7/vNiLwsFUlHvj/BH\nfLiILkGpClCTa9EITvB+NHCZR9O4nFKsPQxyuN72awnFpOcCAwEAAaAAMA0GCSqG\nSIb3DQEBCwUAA4GBACaExWYCwG1u2A0YvVo8tPeKvQ5Z6UgDuLboJxqaMCnZfZLq\nRbJwmsvNPQ+LbiwJmuJnyhpDgBcnnN7ahrp+ZbInz+3bfluzVRAb9ORELGmB2CTm\njhHObJomfu6RBgZCd3jvpbY6RRL7DhYYvnb/Xf+d7Dqip1L3ynWOHaZH+yII";
uint8_t str3[] = "-----BEGIN CERTIFICATE REQUEST-----\nMIIBszCCARwCAQAwczELMAkGA1UEBhMCQ04xEjAQBgNVBAgMCUd1YW5nWmhvdTER\nMA8GA1UEBwwIU2hlblpoZW4xEjAQBgNVBAoMCUNoaW5hVGVhbTEQMA4GA1UECwwH\ndW5pY29ybjEXMBUGA1UEAwwOa2V5bGVzcy5jb20uY24wgZ8wDQYJKoZIhvcNAQEB\nBQADgY0AMIGJAoGBAMR7Lgkdgub8PQKMZFi31XEp04+MWjRNRol3aw0Uj8TeLdwS\nUcuWCAv3+piRpZdOFGhzZPB7voc+R/5acVj7mvnGPrs0xMG3g4fTOiibaBXh+/7z\nYi8LBVJR74/wR3y4iC5BqQpQk2vRCE7wfjRwmUfTuJxSrD0Mcrje9msJxaTnAgMB\nAAGgADANBgkqhkiG9w0BAQsFAAOBgQASj5MgYkaONm5EuXkLSKDh6phjTJLrdVTQ\nJivltqaDdWprcqfR49bs6+0BruyzQNplpm8I2iRD3RJf5FNU7/FFw7LquIXcSptN\nz3hZGYceb2SIOVpHtN/9cRJzgtxPN0b6svGRVsHfo3KAm5I5+hej/5z1exC9yrxg\ngnOlEhx7Kw==\n-----END CERTIFICATE REQUEST-----";
uint8_t str_buff[64];
uint8_t Rx_data[16];
/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/

/* USER CODE BEGIN PV */

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
    if (huart->Instance == USART1)
    {
        if (Rx_data[0] == 0xBF && Rx_data[7] == 0xFB)
        {
            sprintf((char *) str_buff, "received\r\n");
            HAL_UART_Transmit_DMA(&huart1, str3, sizeof str3);
            memset(str_buff, 0, sizeof str_buff);
        }
        HAL_UART_Receive_DMA(&huart1, Rx_data, 8);
    }
}
/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
    /* USER CODE BEGIN 1 */

    /* USER CODE END 1 */

    /* MCU Configuration--------------------------------------------------------*/

    /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
    HAL_Init();

    /* USER CODE BEGIN Init */

    /* USER CODE END Init */

    /* Configure the system clock */
    SystemClock_Config();

    /* USER CODE BEGIN SysInit */

    /* USER CODE END SysInit */

    /* Initialize all configured peripherals */
    MX_GPIO_Init();
    MX_DMA_Init();
    MX_USART1_UART_Init();
    /* USER CODE BEGIN 2 */
    HAL_UART_Receive_DMA(&huart1, Rx_data, 8);

    /* USER CODE END 2 */

    /* Infinite loop */
    /* USER CODE BEGIN WHILE */
    while (1)
    {
        /* USER CODE END WHILE */

        /* USER CODE BEGIN 3 */
    }
    /* USER CODE END 3 */
}

/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
    RCC_OscInitTypeDef RCC_OscInitStruct = {0};
    RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

    /** Initializes the RCC Oscillators according to the specified parameters
    * in the RCC_OscInitTypeDef structure.
    */
    RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
    RCC_OscInitStruct.HSEState = RCC_HSE_ON;
    RCC_OscInitStruct.HSEPredivValue = RCC_HSE_PREDIV_DIV1;
    RCC_OscInitStruct.HSIState = RCC_HSI_ON;
    RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
    RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
    RCC_OscInitStruct.PLL.PLLMUL = RCC_PLL_MUL9;
    if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
    {
        Error_Handler();
    }
    /** Initializes the CPU, AHB and APB buses clocks
    */
    RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK | RCC_CLOCKTYPE_SYSCLK
                                  | RCC_CLOCKTYPE_PCLK1 | RCC_CLOCKTYPE_PCLK2;
    RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
    RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
    RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
    RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

    if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_2) != HAL_OK)
    {
        Error_Handler();
    }
}

/* USER CODE BEGIN 4 */

/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
    /* USER CODE BEGIN Error_Handler_Debug */
    /* User can add his own implementation to report the HAL error return state */
    __disable_irq();
    while (1)
    {
    }
    /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
