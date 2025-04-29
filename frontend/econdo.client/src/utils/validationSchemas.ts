import { z } from "zod";
import parsePhoneNumberFromString from 'libphonenumber-js';

export const passwordSchema = z
    .string()
    .min(8, { message: "Паролата е прекалено къса" })
    .max(20, { message: "Паролата е твърде дълга" })
    .refine((password) => /[A-Z]/.test(password), {
        message: 'Паролата трябва да съдържа поне една главна буква и да бъде на английски език'
    })
    .refine((password) => /[a-z]/.test(password), {
        message: 'Паролата трябва да съдържа поне една малка буква и да бъде на английски език'
    })
    .refine((password) => /[0-9]/.test(password), {
        message: 'Паролата трябва да съдържа поне една цифра'
    });

export const livingPlaceSchema = z
    .string({
        required_error: "Населено място е задължително поле",
        invalid_type_error: "Населено място е задължително поле",
    }).min(1, 'Населено място е задължително поле');

export const birthDateSchema = z
    .date({
        required_error: "Рождената дата е задължително поле", 
        invalid_type_error: "Рождената дата е задължително поле",
    }).refine((data) => {
        let maxDate = new Date();
        maxDate.setUTCFullYear(maxDate.getUTCFullYear() - 16);

        let minDate = new Date(1970, 0);
        return data >= minDate && data <= maxDate;
    }, {
        message: 'Невалидна рождена дата'
    });

export const phoneNumberSchema = z
    .string({
        required_error: "Телефон е задължително поле",
        invalid_type_error: "Телефон е задължително поле",
    })
    .min(1, 'Телефон е задължително поле')
    .transform((arg, ctx) => {
        const phone = parsePhoneNumberFromString(arg, {
          // set this to use a default country when the phone number omits country code
          defaultCountry: 'BG',
          
          // set to false to require that the whole string is exactly a phone number,
          // otherwise, it will search for a phone number anywhere within the string
          extract: false,
        });
      
        // when it's good
        if (phone && phone.isValid()) {
          return phone.number;
        }
      
        // when it's not
        ctx.addIssue({
          code: z.ZodIssueCode.custom,
          message: 'Телефонният номер е невалиден',
        });
        return z.NEVER;
      });;

export const firstNameSchema = z
    .string()
    .min(1, "Първо име е задължително поле")
    .max(30, "Прекалено дълго първо име");

export const lastNameSchema = z
    .string()
    .min(1, "Фамилно име е задължително поле")
    .max(30, "Прекалено дълго фамилно име");

export const middleNameSchema = z
    .string()
    .min(1, "Презиме е задължително поле")
    .max(30, "Прекалено дълго презиме");

export const emailSchema = z
    .string({
        required_error: 'Имейл е задължително поле',
        invalid_type_error: 'Имейл е задължително поле',
    }).email('Невалиден ймейл');

export const privacyPolicySchema = z
    .literal(true, {
        errorMap: () => ({ message: 'Трябва да се съгласите с политиката за поверителност'}),
    });

export const eulaSchema = z
    .literal(true, {
        errorMap: () => ({ message: 'Трябва да се съгласите с общите условия'}),
    });

export const descriptionSchema = z
    .string()
    .max(200, "Прекалено дълго описание");

export const propertySchema = z.object({
    floor: z.string().min(1, "Етаж е задължително поле"),
    number: z.string().min(1, "Номер е задължително поле"),
    propertyType: z.string({
        required_error: 'Видът на имота е задължително поле',
    }).nonempty('Видът на имота е задължително поле'),
    builtArea: z
      .number({
        required_error: "Застроената площ е задължително поле",
        invalid_type_error: "Застроената площ трябва да бъде число",
      })
      .positive("Застроената площ трябва да бъде положително число"),
    idealParts: z
      .number({
        required_error: "Идеалните части е задължително поле",
        invalid_type_error: "Идеалните части трябва да бъдат число",
      })
      .int("Идеалните части трябва да бъдат цяло число")
      .positive("Идеалните части трябва да бъдат положително число"),
  });

export type PropertyFormValues = z.infer<typeof propertySchema>;

export const occupantSchema = z.object({
    firstName: firstNameSchema,
    middleName: middleNameSchema,
    lastName: lastNameSchema,
    occupantType: z.string({
        required_error: 'Типът на контакта е задължително поле',
    }).nonempty('Типът на контакта е задължително поле'),
    email: z.string()
    .email('Невалиден ймейл')
    .optional()
    .or(z.literal('')),
});

export type OccupantFormValues = z.infer<typeof occupantSchema>;
